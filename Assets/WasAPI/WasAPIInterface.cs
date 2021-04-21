
using Assets.WasAPI.ENums;

using System;

using UnityEngine;

using AudioListener = Assets.WasAPI.AudioListener;

namespace Asset.WasAPI
{
    internal class WasAPIInterface : MonoBehaviour
    {
        [SerializeField]
        private int spectrumSize = 1024;
        internal int SpectrumSize { get => spectrumSize; set => spectrumSize = value; }
        [SerializeField]
        private int minFrequency = 20;
        internal int MinFrequency { get => minFrequency; set => minFrequency = value; }
        [SerializeField]
        private int maxFrequency = 20000;
        internal int MaxFrequency { get => maxFrequency; set => maxFrequency = value; }
        [SerializeField]
        private ECaptureType captureType = ECaptureType.Loopback;
        internal ECaptureType CaptureType { get => captureType; set => captureType = value; }
        [SerializeField]
        private Action<float[]> receiveSpectrum;
        internal Action<float[]> ReceiveSpectrum { get => receiveSpectrum; set => receiveSpectrum = value; }

        private AudioListener audioListener;

        // Start is called before the first frame update
        void Start()
        {
            audioListener = new AudioListener(spectrumSize, minFrequency, maxFrequency, receiveSpectrum);
            audioListener.SetCaptureDevice(captureType);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}