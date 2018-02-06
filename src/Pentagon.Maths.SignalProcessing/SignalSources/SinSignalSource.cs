namespace Pentagon.Maths.SignalProcessing.SignalSources {
    using System;
    using Quantities;
    using Units;
    using Units.Converters;

    public class SinSignalSource : ISignalSource
    {
        public SinSignalSource(Frequency sampling, Frequency sinFrequency, double amplitude = 1 , PlaneAngle phase = default(PlaneAngle))
        {
            SamplingFrequency = sampling;
            SinFrequency = sinFrequency;
            Amplitude = amplitude;
            Phase = phase;

            _values = new double[(int)(sampling.Value/sinFrequency.Value)];
            for (var i = 0; i < _values.Length; i++)
            {
                var t = i * sampling.Period;
                _values[i] = amplitude * Math.Sin(2 * Math.PI * sinFrequency.Value * t + (phase.HasValue ? new PlaneAngleUnitConverter().ConvertUnit(phase, new Radian()).Value : 0 ));
            }
        }

        double[] _values;

        /// <inheritdoc />
        public Frequency SamplingFrequency { get; }

        public Frequency SinFrequency { get; }

        public double Amplitude { get; }

        public PlaneAngle Phase { get; }

        /// <inheritdoc />
        public double GetValueAt(int sample)
        {
            var perSample = sample % _values.Length;

            return _values[perSample];
        }
    }
}