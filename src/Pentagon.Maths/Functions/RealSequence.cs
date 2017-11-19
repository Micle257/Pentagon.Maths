namespace Pentagon.Maths.Functions {
    using System;
    using System.Collections;
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

    public class Sequence<TNumber> : IEnumerable<TNumber>
        where TNumber : struct, IEquatable<TNumber>
    {
        IList<TNumber> _values;
        int _zeroIndex;

        public Sequence(IEnumerable<TNumber> values, int zeroIndex)
        {
            _values = values as IList<TNumber> ?? values.ToList();
            _zeroIndex = zeroIndex;
        }

        public Sequence(IEnumerable<TNumber> zeroPositiveValues, IEnumerable<TNumber> negativeValues = null)
        {
            if (negativeValues != null)
            {
                var negatives = negativeValues as IList<TNumber> ?? negativeValues.ToList();
                _values = negatives.Reverse().Concat(zeroPositiveValues).ToList();
                _zeroIndex = negatives.Count;
            }
            else
            {
                _values = zeroPositiveValues as IList<TNumber> ?? zeroPositiveValues.ToList();
                _zeroIndex = 0;
            }
        }

        public TNumber this[int index]
        {
            get
            {
                 if (index + _zeroIndex >= _values.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _values[index + _zeroIndex];
            }
        }

        public IEnumerator<TNumber> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}