// -----------------------------------------------------------------------
//  <copyright file="Elevation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.PathFinding
{
    using System;
    using Quantities;
    using Units;

    /// <summary> Represents an elevation of a geographic location is its height above or below a fixed reference point (geoid sea level). </summary>
    public struct Elevation : IValueDataType<Elevation>
    {
        readonly Length _length;

        /// <summary> Initializes a new instance of the <see cref="Elevation" /> struct. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="unit"> The unit. </param>
        public Elevation(Length length) : this()
        {
            _length = length;
            Value = length.Value;
            HasValue = true;
        }

        /// <summary> Gets a value indicating whether this elevation is above sea level. </summary>
        /// <value> <c> true </c> if it's above sea level; otherwise, <c> false </c>. </value>
        public bool AboveSeaLevel => Value > 0;

        /// <inheritdoc />
        public ILengthUnit Unit { get; }

        /// <inheritdoc />
        public double Value { get; set; }

        /// <inheritdoc />
        public bool HasValue { get; }

        #region Operators

        /// <inheritdoc />
        public static bool operator <(Elevation left, Elevation right) => left.CompareTo(right) < 0;

        /// <inheritdoc />
        public static bool operator >(Elevation left, Elevation right) => left.CompareTo(right) > 0;

        /// <inheritdoc />
        public static bool operator <=(Elevation left, Elevation right) => left.CompareTo(right) <= 0;

        /// <inheritdoc />
        public static bool operator >=(Elevation left, Elevation right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public static bool operator ==(Elevation left, Elevation right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Elevation left, Elevation right) => !left.Equals(right);

        #endregion

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(Elevation other) => Value.Equals(other.Value);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Elevation && Equals((Elevation) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        #endregion

        /// <inheritdoc />
        public int CompareTo(Elevation other) => Value.CompareTo(other.Value);

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is Elevation))
                throw new ArgumentException($"Object must be of type {nameof(Elevation)}");
            return CompareTo((Elevation) obj);
        }

        /// <inheritdoc />
        public override string ToString() => $"Elevation: {Math.Abs(Value)} {(AboveSeaLevel ? "above sea level" : "below sea level")}";
    }
}