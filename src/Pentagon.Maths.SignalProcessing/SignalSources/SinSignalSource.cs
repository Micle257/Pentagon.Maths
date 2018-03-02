// -----------------------------------------------------------------------
//  <copyright file="SinSignalSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SignalSources
{
    using System;
    using Quantities;
    using Units;
    using Units.Converters;

    public class SinSignalSource : ISignalSource
    {
        readonly double[] _values;

        public SinSignalSource(Frequency sampling, Frequency sinFrequency, double amplitude = 1, PlaneAngle phase = default)
        {
            SamplingFrequency = sampling;
            SinFrequency = sinFrequency;
            Amplitude = amplitude;
            Phase = phase;

            _values = new double[(int) (sampling.Value / sinFrequency.Value)];
            for (var i = 0; i < _values.Length; i++)
            {
                var t = i * sampling.Period;
                _values[i] = amplitude * Math.Sin(2 * Math.PI * sinFrequency.Value * t + (phase.HasValue ? new PlaneAngleUnitConverter().ConvertUnit(phase, new Radian()).Value : 0));
            }
        }

        public Frequency SinFrequency { get; }

        public double Amplitude { get; }

        public PlaneAngle Phase { get; }

        /// <inheritdoc />
        public Frequency SamplingFrequency { get; }

        /// <inheritdoc />
        public double GetValueAt(int sample)
        {
            var perSample = sample % _values.Length;

            return _values[perSample];
        }
    }
}