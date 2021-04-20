
using Assets.WasAPI.ENums;

using System;

using UnityEngine;

using AudioListener = Assets.WasAPI.AudioListener;

namespace Asset.WasAPI
{
    public class WasAPIInterface : MonoBehaviour
    {
        [SerializeField]
        private int spectrumSize = 1024;
        public int SpectrumSize { get => spectrumSize; set => spectrumSize = value; }
        [SerializeField]
        private int minFrequency = 20;
        public int MinFrequency { get => minFrequency; set => minFrequency = value; }
        [SerializeField]
        private int maxFrequency = 20000;
        public int MaxFrequency { get => maxFrequency; set => maxFrequency = value; }
        [SerializeField]
        private ECaptureType captureType = ECaptureType.Loopback;
        public ECaptureType CaptureType { get => captureType; set => captureType = value; }
        [SerializeField]
        private Action<float> receiveSpectrum;
        public Action<float> ReceiveSpectrum { get => receiveSpectrum; set => receiveSpectrum = value; }

        private AudioListener audioListener = new AudioListener();

        // Start is called before the first frame update
        void Start()
        {
            audioListener.SetCaptureDevice(captureType);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}