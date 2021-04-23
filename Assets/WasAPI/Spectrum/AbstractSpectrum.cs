using Assets.WasAPI.ENums;
using Assets.WasAPI.Interfaces;

using CSCore;
using CSCore.DSP;

using System;
using System.Collections.Generic;

namespace Assets.WasAPI.Spectrum
{
    internal abstract class AbstractSpectrum
    {
        private const int ScaleFactorLinear = 9;
        protected const int ScaleFactorSqr = 2;
        protected const double MinDbValue = -90;
        protected const double MaxDbValue = 0;
        protected const double DbScale = (MaxDbValue - MinDbValue);

        private int fftSize;
        private bool isXLogScale;
        private int maxFftIndex;
        private readonly int minFrequency;
        private int minimumFrequencyIndex;
        private readonly int maxFrequency;
        private int maximumFrequencyIndex;
        private int[] spectrumIndexMax;
        private int[] spectrumLogScaleIndexMax;
        private ISpectrumProvider spectrumProvider;

        protected int SpectrumResolution;
        private bool _useAverage;

        public ISpectrumProvider SpectrumProvider
        {
            get => spectrumProvider;
            set => spectrumProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool IsXLogScale
        {
            get => isXLogScale;
            set
            {
                isXLogScale = value;
                UpdateFrequencyMapping();
            }
        }

        public EScalingStrategy ScalingStrategy { get; set; }

        public bool UseAverage
        {
            get => _useAverage;
            set => _useAverage = value;
        }

        public FftSize FftSize
        {
            get => (FftSize)fftSize;
            protected set
            {
                if ((int)Math.Log((int)value, 2) % 1 != 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                fftSize = (int)value;
                maxFftIndex = fftSize / 2 - 1;
            }
        }

        public AbstractSpectrum(int minFrequency, int maxFrequency)
        {
            this.minFrequency = minFrequency;
            this.maxFrequency = maxFrequency;
        }

        protected virtual void UpdateFrequencyMapping()
        {
            minimumFrequencyIndex = Math.Min(spectrumProvider.GetFftBandIndex(minFrequency), maxFftIndex);
            maximumFrequencyIndex = Math.Min(spectrumProvider.GetFftBandIndex(maxFrequency) + 1, maxFftIndex);

            var actualResolution = SpectrumResolution;

            var indexCount = maximumFrequencyIndex - minimumFrequencyIndex;
            var linearIndexBucketSize = Math.Round(indexCount / (double)actualResolution, 3);

            spectrumIndexMax = spectrumIndexMax.CheckBuffer(actualResolution, true);
            spectrumLogScaleIndexMax = spectrumLogScaleIndexMax.CheckBuffer(actualResolution, true);

            var maxLog = Math.Log(actualResolution, actualResolution);

            for (int i = 1; i < actualResolution; i++)
            {
                var logIndex =
                    (int)((maxLog - Math.Log((actualResolution + 1) - i, (actualResolution + 1))) * indexCount) +
                    minimumFrequencyIndex;

                spectrumIndexMax[i - 1] = minimumFrequencyIndex + (int)(i * linearIndexBucketSize);
                spectrumLogScaleIndexMax[i - 1] = logIndex;
            }

            if (actualResolution > 0)
            {
                spectrumIndexMax[spectrumIndexMax.Length - 1] =
                    spectrumLogScaleIndexMax[spectrumLogScaleIndexMax.Length - 1] = maximumFrequencyIndex;
            }
        }

        protected virtual SpectrumPointData[] CalculateSpectrumPoints(double maxValue, float[] fftBuffer)
        {
            var dataPoints = new List<SpectrumPointData>();

            double value0 = 0, value = 0;
            double lastValue = 0;
            double actualMaxValue = maxValue;
            var spectrumPointIndex = 0;

            for (int i = minimumFrequencyIndex; i <= maximumFrequencyIndex; i++)
            {
                switch (ScalingStrategy)
                {
                    case EScalingStrategy.Decibel:
                        value0 = (((20 * Math.Log10(fftBuffer[i])) - MinDbValue) / DbScale) * actualMaxValue;
                        break;
                    case EScalingStrategy.Linear:
                        value0 = (fftBuffer[i] * ScaleFactorLinear) * actualMaxValue;
                        break;
                    case EScalingStrategy.Sqrt:
                        value0 = ((Math.Sqrt(fftBuffer[i])) * ScaleFactorSqr) * actualMaxValue;
                        break;
                }

                var recalc = true;

                value = Math.Max(0, Math.Max(value0, value));

                while (spectrumPointIndex <= spectrumIndexMax.Length - 1 &&
                       i ==
                       (IsXLogScale
                           ? spectrumLogScaleIndexMax[spectrumPointIndex]
                           : spectrumIndexMax[spectrumPointIndex]))
                {
                    if (!recalc)
                    {
                        value = lastValue;
                    }

                    if (value > maxValue)
                    {
                        value = maxValue;
                    }

                    if (_useAverage && spectrumPointIndex > 0)
                    {
                        value = (lastValue + value) / 2.0;
                    }

                    dataPoints.Add(new SpectrumPointData { SpectrumPointIndex = spectrumPointIndex, Value = value });

                    lastValue = value;
                    value = 0.0;
                    spectrumPointIndex++;
                    recalc = false;
                }
            }

            return dataPoints.ToArray();
        }

        protected struct SpectrumPointData
        {
            public int SpectrumPointIndex;
            public double Value;
        }
    }
}
