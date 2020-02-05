// -----------------------------------------------------------------------
//  <copyright file="InfiniteDiscreteFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Ranges;

    public class InfiniteDiscreteFunction : IDiscreteFunction
    {
        readonly Func<int, double> _function;

        public InfiniteDiscreteFunction(Func<int, double> function, double samplingFrequency)
        {
            SamplingFrequency = samplingFrequency;
            _function = function;
        }

        public double SamplingFrequency { get; }

        public double this[int sample] => EvaluateSample(sample);

        public double EvaluateSample(int sample) => _function(sample);

        public double EvaluateTime(double time)
        {
            var d = time.ToSampleNumber(SamplingFrequency);
            return EvaluateSample(d);
        }

        public IEnumerable<double> EvaluateSamples(IRange<int> range) => EvaluateSequence(range);

        public static IDiscreteFunction StepFunction(double sampling) => new InfiniteDiscreteFunction(i => i >= 0 ? 1 : 0, sampling);

        public static IDiscreteFunction ImpulseFunction(double sampling) => new InfiniteDiscreteFunction(i => i == 0 ? 1 : 0, sampling);

        public NumberSequence<double> EvaluateSequence(IRange<int> interval)
        {
            var ss = new double[Math.Abs(interval.Max - interval.Min)];
            for (var i = interval.Min; i < interval.Max; i++)
                ss[i] = EvaluateSample(i);
            return new NumberSequence<double>(ss, interval.Min);
        }
    }
}