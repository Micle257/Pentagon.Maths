// -----------------------------------------------------------------------
//  <copyright file="Function.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using JetBrains.Annotations;

    /// <summary> Represents a relation between a set of inputs and a set of permissible outputs with the property that each input is related to exactly one output. </summary>
    public class Function : IRangeable<double>
    {
        public const string IndependentName = "x";

        public Function(FunctionCallback func)
        {
            Func = func;
        }

        public Function(FunctionCallback func, IRange<double> range)
        {
            Func = func;
            Range = range;
        }

        public Function([NotNull] IDictionary<IRange<double>, FunctionCallback> callbacks)
        {
            if (callbacks == null)
                throw new ArgumentNullException(nameof(callbacks));
        }

        public Function(IDictionary<double, double> values)
        {
            Range = new Range<double>(values.Keys.Min(), values.Keys.Max());
            Func = d =>
                   {
                       if (values.ContainsKey(d))
                           return values[d];
                       var key = values.Keys.OrderBy(a => a).Last(va => d > va);
                       return values[key];
                   };
        }

        protected Function() { }

        public FunctionCallback Func { get; }

        public Predicate<double> IsInDomain => d => Range == null || Range.InRange(d);
        
        /// <summary> Gets the range of the Value. </summary>
        public IRange<double> Range { get; }
        
        /// <summary> Gets the output value assigned to input value. </summary>
        /// <param name="x"> The input value of function. </param>
        /// <exception cref="ValueOutOfDomainException"> </exception>
        public virtual double GetValue(double x)
        {

            if (!IsInDomain(x))
                throw new ValueOutOfDomainException(x);
            return Func(x);
        }

        public virtual Function GetDerivative()
        {
            return new Function(d => GetChangeInRate(d, .000001));
        }

        public double GetChangeInRate(double x, double precision)
        {
            if (!(IsInDomain(x) || IsInDomain(x - precision) || IsInDomain(x + precision)))
                throw new ValueOutOfDomainException(x);
            return (Func(x - precision) - Func(x + precision)) / (-2 * precision);
        }

        public double IntegrateApprox(Range<double> b, double incr)
        {
            var result = 0d;
            for (var i = b.Min; i < b.Max - incr; i += incr)
                result += GetValue(i) * incr;
            return result;
        }

        public double IntegrateApprox(Range<double> boundaries, int samples)
        {
            var sum = 0d;
            for (var i = 0; i < samples; i++)
                sum += GetValue(boundaries.Min + i * ((boundaries.Max - boundaries.Min) / samples));
            return sum * (boundaries.Max - boundaries.Min) / samples;
        }

        public IDictionary<double, double> GetValues(Range<double> interval, double step)
        {
            var di = new Dictionary<double, double>();
            for (var i = interval.Min; i < interval.Max; i += step)
                di.Add(i, GetValue(i));
            return di;
        }

        public double[] GetSamples(int sampleCount, double samplingFrequency, double startTime = 0)
        {
            var samples = new double[sampleCount];
            var dt = 1/ samplingFrequency;
            var t = startTime;
            for (var i = 0; i < sampleCount; i++, t += dt)
                samples[i] = GetValue(t);
            return samples;
        }

        public IDiscreteFunction ToDiscreteFunction(double samplingFrequency, MathInterval mathInterval)
        {
            var startTime = Math.Abs(mathInterval.Min.ToSample(samplingFrequency));
            var count = (int) (mathInterval.Size * samplingFrequency);
            return new RealSequence(GetSamples(count + 1, samplingFrequency, mathInterval.Min), startTime, samplingFrequency);
        }

        public IDiscreteFunction ToDiscreteFunction(double sampl) => new InfiniteDiscreteFunction(i => GetValue(i.ToTime(sampl)), sampl);
    }
}