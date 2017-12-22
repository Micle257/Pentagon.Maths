// -----------------------------------------------------------------------
//  <copyright file="Percentage.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using Helpers;
    using Pentagon.Extensions;

    public struct Percentage : IEquatable<Percentage>, IComparable<Percentage>, IRangeable<double>
    {
        public Percentage(double doubleValue) : this()
        {
            if (Range.InRange(doubleValue))
                throw new ArgumentOutOfRangeException(nameof(doubleValue));
            DecimalValue = doubleValue;
        }

        /// <summary> Gets the decimal value, ranging 0 to 1. </summary>
        public double DecimalValue { get; }

        /// <summary> Gets the value of percentage, ranging 0 to 100. </summary>
        public double Value => DecimalValue * 100;

        public IRange<double> Range => new MathInterval(0, 1);

        #region Operators

        public static implicit operator double(Percentage value) => value.DecimalValue;

        public static explicit operator Percentage(double value) => new Percentage(value);

        public static bool operator ==(Percentage left, Percentage right) => left.Equals(right);

        public static bool operator !=(Percentage left, Percentage right) => !left.Equals(right);

        #endregion

        public int CompareTo(Percentage other) => DecimalValue.CompareTo(other.DecimalValue);

        public bool Equals(Percentage other) => DecimalValue.EqualTo(other.DecimalValue);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Percentage && Equals((Percentage) obj);
        }

        public override int GetHashCode() => DecimalValue.GetHashCode();

        public override string ToString() => $"{DecimalValue.RoundSignificantFigures(3) * 100}%";
    }
}