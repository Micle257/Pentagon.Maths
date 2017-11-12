namespace Pentagon.Maths.Functions {
    using System;
    using Helpers;
    using Quantities;

    public class InfiniteDiscreteFunction : IDiscreteFunction
    {
        readonly Func<int, double> _function;

        public InfiniteDiscreteFunction(Func<int, double> function, Frequency samplingFrequency)
        {
            SamplingFrequency = samplingFrequency;
            _function = function;
        }

        public static IDiscreteFunction StepFunction(Frequency sampling) => new InfiniteDiscreteFunction(i => i >= 0 ? 1 : 0, sampling);

        public static IDiscreteFunction ImpulseFunction(Frequency sampling) => new InfiniteDiscreteFunction(i => i == 0 ? 1 : 0, sampling);

        public Frequency SamplingFrequency { get; }

        public double this[int sample] => EvaluateSample(sample);

        public double EvaluateSample(int sample)
        {
            return _function(sample);
        }

        public double EvaluateTime(double time)
        {
            var d = time.ToSample(SamplingFrequency);
            return EvaluateSample(d);
        }

        public Sequence<double> EvaluateSequence(IRange<int> interval)
        {
            var ss = new double[Math.Abs(interval.Max - interval.Min)];
            for (var i = interval.Min; i < interval.Max; i++)
                ss[i] = EvaluateSample(i);
            return new Sequence<double>(ss, interval.Min);
        }
    }
}