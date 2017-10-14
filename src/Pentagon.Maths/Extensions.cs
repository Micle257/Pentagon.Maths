// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Helpers;
    using Quantities;

    public static class Extensions
    {
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

        public static int ToSample(this double val, Frequency sampling) => (int) (val * sampling.Value);

        public static double ToTime(this int val, Frequency sampling) => val / sampling.Value;

        public static double Cbrt(this double v)
        {
            if (v >= 0)
                return Math.Pow(v, 1d / 3d);
            var s = -v;
            return -Math.Pow(s, 1d / 3d);
        }
    }
}