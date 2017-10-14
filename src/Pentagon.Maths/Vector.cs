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

    public class Vector : IEnumerable<double>
    {
        readonly IList<double> _values;

        public Vector(IEnumerable<double> values)
        {
            if (values is IList<double> v)
                _values = v;
            else
                _values = values.ToList();
        }

        public double[] Values => _values.ToArray();

        public int Length => _values.Count;

        public double this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        #region Operators

        public static Vector operator +(Vector a, Vector b)
        {
            var max = new[] {a.Length, b.Length}.Max();
            var av = a.Values;
            var bv = b.Values;
            Array.Resize(ref av, max);
            Array.Resize(ref bv, max);
            var sum = new double[max];
            for (var i = 0; i < max; i++)
                sum[i] += av[i] + bv[i];
            return new Vector(sum);
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<double> GetEnumerator() => _values.GetEnumerator();

        public Vector Convolution(Vector v)
        {
            var m = Length;
            var n = v.Length;
            var w = new double[m + n - 1];
            for (var k = 0; k < w.Length; k++)
            {
                for (var j = new[] {0, k - n + 1}.Max(); j < new[] {k + 1, m}.Min(); j++) // var j = max(1,k+1-n); j < min(k,m); j++
                    w[k] += this[j] * v[k - j];
            }
            return new Vector(w);
        }
    }
}