// -----------------------------------------------------------------------
//  <copyright file="Vector.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SignalProcessing;

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
        
        public Polynomial GetPolynomial() => new Polynomial(Values);

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
            return new Signal((a.GetPolynomial() + b.GetPolynomial()).Coefficients);
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<double> GetEnumerator() => _values.GetEnumerator();
    }
}