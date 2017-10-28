// -----------------------------------------------------------------------
//  <copyright file="MathPoint.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using Pentagon.Extensions;

    public struct MathPoint : IValueDataType<MathPoint>
    {
        public MathPoint(double x, double y)
        {
            X = x;
            Y = y;
            HasValue = true;
        }

        public double X { get; }
        public double Y { get; }

        /// <inheritdoc />
        public bool HasValue { get; }

        #region Operators

        /// <inheritdoc />
        public static bool operator <(MathPoint left, MathPoint right) => left.CompareTo(right) < 0;

        /// <inheritdoc />
        public static bool operator >(MathPoint left, MathPoint right) => left.CompareTo(right) > 0;

        /// <inheritdoc />
        public static bool operator <=(MathPoint left, MathPoint right) => left.CompareTo(right) <= 0;

        /// <inheritdoc />
        public static bool operator >=(MathPoint left, MathPoint right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public static bool operator ==(MathPoint left, MathPoint right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(MathPoint left, MathPoint right) => !left.Equals(right);

        #endregion

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(MathPoint other) => X.EqualTo(other.X) && Y.EqualTo(other.Y);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is MathPoint && Equals((MathPoint) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        #endregion

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is MathPoint))
                throw new ArgumentException($"Object must be of type {nameof(MathPoint)}");
            return CompareTo((MathPoint) obj);
        }

        /// <inheritdoc />
        public int CompareTo(MathPoint other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0)
                return xComparison;
            return Y.CompareTo(other.Y);
        }

        /// <inheritdoc />
        public override string ToString() => $"{Math.Round(X, 5)}; {Math.Round(Y, 5)}";
    }
}