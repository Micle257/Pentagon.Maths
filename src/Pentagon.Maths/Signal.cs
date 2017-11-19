// -----------------------------------------------------------------------
//  <copyright file="Vector.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    public class RelativeSignal
    {
        [NotNull]
        readonly Signal _signal;

        public RelativeSignal([NotNull] Signal signal)
        {
            Require.NotNull(() => signal);
            _signal = signal;
        }

        public double this[int delay]
        {
            get
            {
                if (_signal.Length + delay < 0 || _signal.Length + delay >= _signal.Length)
                    return 0;
                else
                    return _signal[_signal.Length + delay];
            }
        }
    }

    public class SignalBuilder
    {
        ICollection<double> _values = new List<double>();

        public void AddSample(double sample)
        {
            _values.Add(sample);
        }

        [NotNull]
        public Signal GetSignal() => new Signal(_values);

        public RelativeSignal GetRelativeSignal()
        {
          return new RelativeSignal(GetSignal());
        }

        public void AddSignal(Signal signal)
        {
            foreach (var d in signal)
            {
                AddSample(d);
            }
        }
    }

    public class Signal : IEnumerable<double>
    {
        readonly IList<double> _values;

        public Signal(IEnumerable<double> values)
        {
            if (values is IList<double> v)
                _values = v;
            else
                _values = values.ToList();
        }

        public double[] Values => _values.ToArray();

        public int Length => _values.Count;

        public double this[int sample]
        {
            get => _values[sample];
            set => _values[sample] = value;
        }

        #region Operators

        public static Signal operator +(Signal a, Signal b)
        {
            return new Signal(Polynomial.Add(a,b));
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<double> GetEnumerator() => _values.GetEnumerator();

        public Signal Convolution(Signal v)
        {
         return  new Signal(Polynomial.Convolution(this, v));
        }
    }

    public class Polynomial
    {
        public static IEnumerable<double> Add(IEnumerable<double> first, IEnumerable<double> second)
        {
            var av = first.ToArray();
            var bv = second.ToArray();

            var max = new[] { av.Length, bv.Length }.Max();
            Array.Resize(ref av, max);
            Array.Resize(ref bv, max);
            var sum = new double[max];
            for (var i = 0; i < max; i++)
                sum[i] += av[i] + bv[i];
            return sum;
        }

        public static IEnumerable<double> Convolution(IEnumerable<double> first, IEnumerable<double> second)
        {
            var av = first.ToArray();
            var bv = second.ToArray();

            var m = av.Length;
            var n = bv.Length;
            var w = new double[m + n - 1];
            for (var k = 0; k < w.Length; k++)
            {
                for (var j = new[] { 0, k - n + 1 }.Max(); j < new[] { k + 1, m }.Min(); j++)
                    w[k] += av[j] * bv[k - j];
            }
            return w;
        }
    }
}