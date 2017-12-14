namespace Pentagon.Maths.Functions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Quantities;

    public class RealSequence : Sequence<double>, IDiscreteFunction
    {
        public RealSequence(IEnumerable<double> values, int zeroIndex, Frequency samplingFrequency) : base(values, zeroIndex)
        {
            SamplingFrequency = samplingFrequency;
        }
        public RealSequence(IEnumerable<double> zeroPositiveValues, IEnumerable<double> negativeValues = null) : base(zeroPositiveValues, negativeValues) { }
        public Frequency SamplingFrequency { get; }
        public double EvaluateSample(int sample)
        {
            if (sample + _zeroIndex >= this.Count() || sample + _zeroIndex < 0)
                return 0;
            return this[sample];
        }

        public double EvaluateTime(double time)
        {
            var d = time.ToSample(SamplingFrequency);
            return EvaluateSample(d);
        }

        public IEnumerable<double> EvaluateSamples(IRange<int> range)
        {
            var ss = new double[Math.Abs(range.Max - range.Min)];
            for (var i = range.Min; i < range.Max; i++)
                ss[i] = EvaluateSample(i);
            return ss;
        }
    }
}