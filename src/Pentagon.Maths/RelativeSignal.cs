// -----------------------------------------------------------------------
//  <copyright file="RelativeSignal.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct RelativeSignal
    {
        readonly List<double> _values;

        public RelativeSignal(int size)
        {
            Size = size;

            _values = new List<double>(Enumerable.Repeat(0d, size));
        }

        public int Size { get; }

        public double this[int delay]
        {
            get
            {
                if (delay > 0)
                    throw new ArgumentOutOfRangeException(nameof(delay));

                if (-delay > Size)
                    throw new ArgumentOutOfRangeException(nameof(delay));

                return _values[Size + delay - 1];
            }
        }

        public void AddSample(double value)
        {
            //_queue.Requeue(value);
            _values.RemoveAt(0);
            _values.Add(value);
        }

        public void SetLastSample(double value)
        {
            _values[_values.Count - 1] = value;
        }
    }
}