// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using Helpers;
    using JetBrains.Annotations;

    public static class Extensions
    {
        static readonly HashSet<Type> NumericTypes = new HashSet<Type>
                                                     {
                                                             typeof(short),
                                                             typeof(ushort),
                                                             typeof(byte),
                                                             typeof(sbyte),
                                                             typeof(int),
                                                             typeof(uint),
                                                             typeof(long),
                                                             typeof(ulong),
                                                             typeof(float),
                                                             typeof(double),
                                                             typeof(decimal),
                                                             typeof(BigInteger),
                                                             typeof(Complex)
                                                     };

        public static bool IsNumeric(this object value) => NumericTypes.Contains(value.GetType()) || NumericTypes.Contains(Nullable.GetUnderlyingType(value.GetType()));

        public static bool InRange(this int val, int min, int max) => new Range<int>(min, max).InRange(val);

        public static bool InRange(this double val, double min, double max) => new MathInterval(min, max).InRange(val);

        public static bool InRange(this double val, MathInterval intv) => intv.InRange(val);

        public static double Round(this double value, int decimals) => Math.Round(value, decimals);

        public static double ToDouble(this string val)
        {
            if (val == null)
                throw new ArgumentNullException();
            if (val == string.Empty || !val.ToCharArray().Any(char.IsDigit))
                return 0;
            if (val.ToCharArray().Last().ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                return Convert.ToDouble(val + "0");
            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                val = val.Replace(',', '.');
            else if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                val = val.Replace('.', ',');
            return Convert.ToDouble(val);
        }

        public static int ToSample(this double val, double sampling) => (int) (val * sampling);

        public static double ToTime(this int val, double sampling) => val / sampling;

        public static double Cbrt(this double v)
        {
            if (v >= 0)
                return Math.Pow(v, 1d / 3d);
            var s = -v;
            return -Math.Pow(s, 1d / 3d);
        }

        /// <summary> Computes absolute value of each value in the collection. </summary>
        /// <param name="collection"> The collection. </param>
        /// <returns> An enumeration of absolute values. </returns>
        /// <exception cref="ArgumentNullException"> When collection is null. </exception>
        public static IEnumerable<double> Abs([NotNull] this IEnumerable<double> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var value in collection)
            {
                if (value < 0d)
                    yield return Math.Abs(value);
                else
                    yield return value;
            }
        }
    }
}