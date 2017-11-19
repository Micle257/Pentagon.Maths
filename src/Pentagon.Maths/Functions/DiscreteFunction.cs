// -----------------------------------------------------------------------
//  <copyright file="DiscreteFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Helpers;
    using Quantities;
    using SignalProcessing;

    public class DiscreteFunction : IDiscreteFunction
    {
        readonly Func<int, double> _func;
        readonly double _initialValue;

        public DiscreteFunction(Func<int,double> function,Frequency sampling, double initialValue = 0)
        {
            SamplingFrequency = sampling;
            _func = function;
            _initialValue = initialValue;
        }
        
        public double EvaluateTime(double t)
        {
            var d = t.ToSample(SamplingFrequency);
            return EvaluateSample(d);
        }

        public Frequency SamplingFrequency { get; }

        public double this[int sample]
        {
            get { throw new NotImplementedException(); }
        }

        public double EvaluateSample(int n) => _initialValue + _func(n);

        public IEnumerable<double> EvaluateSamples(IRange<int> range)
        {
            var ss = new double[Math.Abs(range.Max - range.Min)];
            for (var i = range.Min; i < range.Max; i++)
                ss[i] = EvaluateSample(i);
            return ss;
        }
    }
}