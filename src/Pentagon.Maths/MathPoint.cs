// -----------------------------------------------------------------------
//  <copyright file="MathPoint.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using Pentagon.Extensions;

    public struct MathPoint : IEquatable<MathPoint>, IComparable<MathPoint>, IComparable
    {
        public MathPoint(double x, double y)
        {
            X = x;
            Y = y;
            IsDefault = false;
        }

        public bool IsDefault { get; }
        public double X { get; set; }
        public double Y { get; set; }

        #region Operators

        /// <summary>
        ///     Returns a value that indicates whether a <see cref="T:Pentagon.Maths.MathPoint" /> value is less than another
        ///     <see cref="T:Pentagon.Maths.MathPoint" /> value.
        /// </summary>
        /// <param name="left"> The first value to compare. </param>
        /// <param name="right"> The second value to compare. </param>
        /// <returns> true if <paramref name="left" /> is less than <paramref name="right" />; otherwise, false. </returns>
        public static bool operator <(MathPoint left, MathPoint right) => left.CompareTo(right) < 0;

        /// <summary> Returns a value that indicates whether a <see cref="T:Pentagon.Maths.MathPoint" /> value is greater than another <see cref="T:Pentagon.Maths.MathPoint" /> value. </summary>
        /// <param name="left"> The first value to compare. </param>
        /// <param name="right"> The second value to compare. </param>
        /// <returns> true if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, false. </returns>
        public static bool operator >(MathPoint left, MathPoint right) => left.CompareTo(right) > 0;

        /// <summary>
        ///     Returns a value that indicates whether a <see cref="T:Pentagon.Maths.MathPoint" /> value is less than or equal to another <see cref="T:Pentagon.Maths.MathPoint" /> value.
        /// </summary>
        /// <param name="left"> The first value to compare. </param>
        /// <param name="right"> The second value to compare. </param>
        /// <returns> true if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, false. </returns>
        public static bool operator <=(MathPoint left, MathPoint right) => left.CompareTo(right) <= 0;

        /// <summary>
        ///     Returns a value that indicates whether a <see cref="T:Pentagon.Maths.MathPoint" /> value is greater than or equal to another <see cref="T:Pentagon.Maths.MathPoint" /> value.
        /// </summary>
        /// <param name="left"> The first value to compare. </param>
        /// <param name="right"> The second value to compare. </param>
        /// <returns> true if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, false. </returns>
        public static bool operator >=(MathPoint left, MathPoint right) => left.CompareTo(right) >= 0;

        public static bool operator ==(MathPoint left, MathPoint right) => left.Equals(right);

        public static bool operator !=(MathPoint left, MathPoint right) => !left.Equals(right);

        #endregion

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is MathPoint))
                throw new ArgumentException($"Object must be of type {nameof(MathPoint)}");
            return CompareTo((MathPoint) obj);
        }

        public int CompareTo(MathPoint other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0)
                return xComparison;
            return Y.CompareTo(other.Y);
        }

        public bool Equals(MathPoint other) => X.EqualTo(other.X) && Y.EqualTo(other.Y);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is MathPoint && Equals((MathPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString() => $"{Math.Round(X, 5)}; {Math.Round(Y, 5)}";
    }
}