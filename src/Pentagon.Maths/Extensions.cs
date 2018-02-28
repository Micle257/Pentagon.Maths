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

    public static class DoubleExtensions
    {
        public static double Round(this double value, int decimals) => Math.Round(value, decimals);

        public static bool InRange(this double val, double min, double max) => new MathInterval(min, max).InRange(val);

        public static double? ToDouble(this string val)
        {
            if (val == null)
                throw new ArgumentNullException();

            if (val.ToCharArray().Last().ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                val = val.Remove(val.Length - 1);

            if (!double.TryParse(val, out var doubleValue))
                return null;

            return doubleValue;
        }

        public static double Cbrt(this double value)
        {
            if (value >= 0)
                return Math.Pow(value, 1d / 3d);

            var s = -value;
            return -Math.Pow(s, 1d / 3d);
        }

        public static int ToSampleNumber(this double value, double samplingFrequency) => (int)(value * samplingFrequency);

        public static double ToTimeValue(this int value, double samplingFrequency) => value / samplingFrequency;

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

    public static class Extensions
    {
      public  static readonly HashSet<Type> NumericTypes = new HashSet<Type>
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

        public static decimal Round(this decimal value, int decimals) => Math.Round(value, decimals);
    }
}