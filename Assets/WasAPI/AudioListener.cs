using Assets.WasAPI.ENums;

using CSCore.CoreAudioAPI;
using CSCore.SoundIn;

namespace Assets.WasAPI
{
    public class AudioListener
    {
        private ECaptureType captureType = ECaptureType.Undefined;
        private WasapiCapture capture;

        public void StartListen()
        {

        }

        public void StopListen()
        {
            if (capture.RecordingState == RecordingState.Recording)
                capture.Stop();
            capture.Dispose();
        }

        public void SetCaptureDevice(ECaptureType captureType)
        {
            if (this.captureType != captureType)
            {
                this.captureType = captureType;
                StopListen();
                SetupWasapiCapture();
                if (this.captureType != ECaptureType.Undefined)
                    StartListen();
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
    }
}
