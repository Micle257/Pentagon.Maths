// -----------------------------------------------------------------------
//  <copyright file="Sum.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Numerics;

    public class Sum
    {
        public static double Compute(int from, int to, Func<int, double> function)
        {
            var sum = 0d;
            for (int i = from; i < to; i++)
                sum += function(i);
            return sum;
        }

        public static Complex ComputeComplex(int from, int to, Func<int, Complex> function)
        {
            var sum = default(Complex);
            for (int i = from; i < to; i++)
                sum += function(i);
            return sum;
        }
    }
}