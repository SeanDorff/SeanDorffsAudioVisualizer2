using Assets.WasAPI.ENums;
using Assets.WasAPI.Spectrum;
using Assets.WasAPI.SpectrumProvider;

using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;

using System;

using static Assets.WasAPI.Consts.WasAPIConsts;

namespace Assets.WasAPI
{
    internal class AudioListener
    {
        private ECaptureType captureType = ECaptureType.Undefined;
        private WasapiCapture capture;
        private SoundInSource soundInSource;
        private BasicSpectrumProvider basicSpectrumProvider;
        private LineSpectrum lineSpectrum;
        private SingleBlockNotificationStream singleBlockNotificationStream;
        private IWaveSource realtimeSource;
        private int spectrumSize;
        private int minFrequency;
        private int maxFrequency;
        private Action<float[]> receiveAudio;

        internal AudioListener(int spectrumSize, int minFrequency, int maxFrequency, Action<float[]> receiveAudio)
        {
            this.spectrumSize = spectrumSize;
            this.minFrequency = minFrequency;
            this.maxFrequency = maxFrequency;
            this.receiveAudio = receiveAudio;
        }

        internal void StartListen()
        {
            capture.Initialize();
            soundInSource = new SoundInSource(capture);
            basicSpectrumProvider = new BasicSpectrumProvider(soundInSource.WaveFormat.Channels, soundInSource.WaveFormat.SampleRate, C_FftSize);
            lineSpectrum = new LineSpectrum(C_FftSize, minFrequency, maxFrequency)
            {
                SpectrumProvider = basicSpectrumProvider,
                BarCount = spectrumSize,
                UseAverage = true,
                IsXLogScale = true,
                ScalingStrategy = EScalingStrategy.Sqrt
            };

            capture.Start();

            ISampleSource sampleSource = soundInSource.ToSampleSource();

            singleBlockNotificationStream = new SingleBlockNotificationStream(sampleSource);
            realtimeSource = singleBlockNotificationStream.ToWaveSource();

            byte[] buffer = new byte[realtimeSource.WaveFormat.BytesPerSecond / 128];

            soundInSource.DataAvailable += (s, ea) =>
            {
                while (realtimeSource.Read(buffer, 0, buffer.Length) > 0)
                {
                    var spectrumData = lineSpectrum.GetSpectrumData(C_MaxAudioValue);

                    if (spectrumData != null)
                    {
                        receiveAudio?.Invoke(spectrumData);
                    }
                }
            };

            singleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
        }

        internal void StopListen()
        {
            if (capture != null)
            {
                if (capture.RecordingState == RecordingState.Recording)
                    capture.Stop();
                capture.Dispose();
            }
        }

        internal void SetCaptureDevice(ECaptureType captureType)
        {
            if (this.captureType != captureType)
            {
                this.captureType = captureType;
                StopListen();
                SetupWasapiCapture();
            }
        }

        private void SetupWasapiCapture()
        {
            switch (this.captureType)
            {
                case ECaptureType.Microphone:
                    MMDevice defaultMicrophone;
                    using (MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator())
                    {
                        defaultMicrophone = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                    }
                    capture = new WasapiCapture
                    {
                        Device = defaultMicrophone
                    };
                    break;
                default: // ECaptureType.Loopback
                    capture = new WasapiLoopbackCapture();
                    break;
            }
        }

        private void SingleBlockNotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            basicSpectrumProvider.Add(e.Left, e.Right);
        }
    }
}
