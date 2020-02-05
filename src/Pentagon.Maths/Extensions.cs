// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Helpers;
    using Ranges;

    public static class Extensions
    {
        public static readonly HashSet<Type> NumericTypes = new HashSet<Type>
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