// -----------------------------------------------------------------------
//  <copyright file="Sequence.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Sequence<TNumber> : IEnumerable<TNumber>
            where TNumber : struct, IEquatable<TNumber>
    {
        protected int _zeroIndex;
        readonly IList<TNumber> _values;

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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}