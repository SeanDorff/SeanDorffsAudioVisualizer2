using CSCore.DSP;

using System.Collections.Generic;
using System.Linq;

namespace Assets.WasAPI.Spectrum
{
    internal class LineSpectrum : AbstractSpectrum
    {
        public int BarCount
        {
            get => SpectrumResolution;
            set => SpectrumResolution = value;
        }

        public LineSpectrum(FftSize fftSize, int minFrequency, int maxFrequency)
        : base(minFrequency, maxFrequency)
        {
            FftSize = fftSize;
        }

        public float[] GetSpectrumData(double maxValue)
        {
            // Get spectrum data internal
            var fftBuffer = new float[(int)FftSize];

            UpdateFrequencyMapping();

            if (SpectrumProvider.GetFftData(fftBuffer, this))
            {
                SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(maxValue, fftBuffer);

                // Convert to float[]
                List<float> spectrumData = new List<float>();
                spectrumPoints.ToList().ForEach(point => spectrumData.Add((float)point.Value));
                return spectrumData.ToArray();
            }

            return null;
        }
    }

}
