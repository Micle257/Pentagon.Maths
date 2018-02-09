// -----------------------------------------------------------------------
//  <copyright file="Bit.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;

    /// <summary> Represents a boolean bit value. </summary>
    public struct Bit : IValueDataType<Bit>, IValuable<bool?>
    {
        /// <summary> The inner bit value as integer. </summary>
        readonly bool _bit;

        /// <summary> Initializes a new instance of the <see cref="Bit" /> class. </summary>
        /// <param name="value"> The bit value. </param>
        public Bit(bool value)
        {
            Value = value;
            _bit = value;
            HasValue = true;
        }

        /// <summary> Gets the zero value. </summary>
        /// <value> The <see cref="Bit" />. </value>
        public static Bit Zero => new Bit(false);

        /// <summary> Gets the one value. </summary>
        /// <value> The <see cref="Bit" />. </value>
        public static Bit One => new Bit(true);

        /// <summary> Gets the indeterminate state of bit (default value). </summary>
        /// <value> The <see cref="Bit" />. </value>
        public static Bit Indeterminate => default(Bit);

        /// <inheritdoc />
        public bool? Value { get; }

        /// <inheritdoc />
        public bool HasValue { get; }

        #region Operators

        /// <inheritdoc />
        public static bool operator ==(Bit left, Bit right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Bit left, Bit right) => !left.Equals(right);

        /// <inheritdoc />
        public static bool operator <(Bit left, Bit right) => left.CompareTo(right) < 0;

        /// <inheritdoc />
        public static bool operator >(Bit left, Bit right) => left.CompareTo(right) > 0;

        /// <inheritdoc />
        public static bool operator <=(Bit left, Bit right) => left.CompareTo(right) <= 0;

        /// <inheritdoc />
        public static bool operator >=(Bit left, Bit right) => left.CompareTo(right) >= 0;

        /// <summary> Performs an implicit conversion from <see cref="System.Boolean" /> to <see cref="Bit" />. </summary>
        /// <param name="value"> The boolean representation of bit. </param>
        /// <returns> The result of the conversion. </returns>
        public static implicit operator Bit(bool value) => new Bit(value);

        #endregion

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(Bit other) => Value == other.Value;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Bit && Equals((Bit) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        #endregion

        /// <inheritdoc />
        public int CompareTo(Bit other) => Nullable.Compare(Value, other.Value);

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is Bit))
                throw new ArgumentException($"Object must be of type {nameof(Bit)}");
            return CompareTo((Bit) obj);
        }

        /// <inheritdoc />
        public override string ToString() => $"{(_bit ? "1" : "0")}";
    }
}