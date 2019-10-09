// -----------------------------------------------------------------------
//  <copyright file="SystemTuple.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct SystemTuple
    {
        public SystemTuple(IEnumerable<double> numeratorCoefficients, IEnumerable<double> denumeratorCoefficients)
        {
            var num = numeratorCoefficients as double[] ?? numeratorCoefficients?.ToArray();
            var den = denumeratorCoefficients as double[] ?? denumeratorCoefficients?.ToArray();

            if (num == null || num.Length == 0)
                num = new[] {1d};

            if (den == null || den.Length == 0)
                den = new[] {1d};

            if (den.Length != num.Length)
            {
                if (num.Length < den.Length)
                    Array.Resize(ref num, den.Length);
                else
                    Array.Resize(ref den, num.Length);
            }

            Numerator = num;
            Denumerator = den;

            Order = num.Length;
        }

        public double[] Numerator { get; }

        public double[] Denumerator { get; }

        public int Order { get; }
    }
}