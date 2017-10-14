// -----------------------------------------------------------------------
//  <copyright file="Convolution.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System.Linq;

    /// <summary>
    /// Provides helper methods for number arrays.
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// Computes the convolution of two arrays.
        /// </summary>
        /// <param name="u">The first array.</param>
        /// <param name="v">The second array.</param>
        /// <returns>A result array.</returns>
        public static double[] Convolute(double[] u, double[] v)
        {
            var m = u.Length;
            var n = v.Length;
            var w = new double[m + n - 1];
            for (var k = 0; k < w.Length; k++)
            {
                for (var j = new[] {0, k - n + 1}.Max(); j < new[] {k + 1, m}.Min(); j++) // var j = max(1,k+1-n); j < min(k,m); j++
                    w[k] += u[j] * v[k - j];
            }
            return w;
        }
    }
}