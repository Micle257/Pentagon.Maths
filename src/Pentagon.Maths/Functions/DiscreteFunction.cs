// -----------------------------------------------------------------------
//  <copyright file="DiscreteFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using System;
    using Helpers;
    using Quantities;
    using SignalProcessing;

    public class DiscreteFunction
    {
        readonly Func<int, double> _func;

        public DiscreteFunction(Frequency sampling, double[] samples, int midpoint)
        {
            Samples = samples;
            Midpoint = midpoint;
            Sampling = new SamplingSource(sampling, samples.Length);
            _func = i => Samples[i];
        }

        internal DiscreteFunction(Func<int, double> func, Frequency f)
        {
            Sampling = new SamplingSource(f, TimeSpan.MaxValue);
            _func = func;
        }

        public double[] Samples { get; }
        public int Midpoint { get; }
        public Vector Vector => new Vector(Samples);

        public SamplingSource Sampling { get; }

        public static DiscreteFunction StepFunction(int midpoint = 0) => new DiscreteFunction(i => i >= midpoint ? 1 : 0, Frequency.Infinity);

        public static DiscreteFunction ImpulseFunction(int midpoint = 0) => new DiscreteFunction(i => i == midpoint ? 1 : 0, Frequency.Infinity);

        public double EvaluateTime(double t)
        {
            var d = t.ToSample(Sampling.Frequency);
            return EvaluateSample(d);
        }

        public double EvaluateSample(int n) => _func(n);

        public double[] EvaluateSamples(Range<int> range)
        {
            var ss = new double[Math.Abs(range.Max - range.Min)];
            for (var i = range.Min; i < range.Max; i++)
                ss[i] = EvaluateSample(i);
            return ss;
        }
    }
}