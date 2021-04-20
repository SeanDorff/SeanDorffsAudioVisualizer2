
using Assets.WasAPI.ENums;

using System;

using UnityEngine;

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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}