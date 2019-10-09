// -----------------------------------------------------------------------
//  <copyright file="Frequency.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using System;
    using Helpers;
    using Pentagon.Extensions;
    using Units;
    using Units.Converters;

    /// <summary> Represents the number of occurrences of a repeating event per unit time. </summary>
    public struct Frequency : IPhysicalQuantity, IRangeable<double>, IValueDataType<Frequency>, IUnitConvertable<Frequency>
    {
        /// <summary> Initializes a new instance of the <see cref="Frequency" /> by given value. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="unit"> The unit. </param>
        public Frequency(double value, IPhysicalUnit unit = null) : this()
        {
            if (!Range.InRange(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
            Unit = unit;
            HasValue = true;
            if (Unit == null)
                Unit = new Hertz();
        }

        /// <summary> Gets the infinity value of <see cref="Frequency" />. </summary>
        /// <value> The <see cref="Frequency" />. </value>
        public static Frequency Infinity => new Frequency(double.PositiveInfinity);

        /// <summary> Gets the period of this frequency. </summary>
        /// <value> The <see cref="double" />. </value>
        public double Period => 1 / Value;

        /// <inheritdoc />
        public IPhysicalUnit Unit { get; }

        /// <inheritdoc />
        public IRange<double> Range => new MathInterval(0, double.PositiveInfinity);

        /// <inheritdoc />
        public IPhysicalConversionReferenceUnit ReferenceUnit => new Hertz();

        /// <inheritdoc />
        public double Value { get; }

        /// <inheritdoc />
        public bool HasValue { get; }

        #region Operators

        /// <inheritdoc />
        public static bool operator <(Frequency left, Frequency right) => left.CompareTo(right) < 0;

        /// <inheritdoc />
        public static bool operator >(Frequency left, Frequency right) => left.CompareTo(right) > 0;

        /// <inheritdoc />
        public static bool operator <=(Frequency left, Frequency right) => left.CompareTo(right) <= 0;

        /// <inheritdoc />
        public static bool operator >=(Frequency left, Frequency right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public static bool operator ==(Frequency left, Frequency right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Frequency left, Frequency right) => !left.Equals(right);

        /// <summary> Performs an implicit conversion from <see cref="Frequency" /> to <see cref="System.Double" />. </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The result of the conversion. </returns>
        public static implicit operator double(Frequency value) => value.Value;

        /// <summary> Performs an explicit conversion from <see cref="System.Double" /> to <see cref="Frequency" />. </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The result of the conversion. </returns>
        public static explicit operator Frequency(double value) => new Frequency(value);

        /// <summary> Performs an additive operation over two frequencies. </summary>
        /// <param name="left"> The left. </param>
        /// <param name="right"> The right. </param>
        /// <returns> The result of the operator. </returns>
        public static Frequency operator +(Frequency left, Frequency right) => new Frequency(left.Value + right.Value);

        public static Frequency operator *(Frequency left, double factor) => new Frequency(left.Value * factor, left.Unit);

        public static Frequency operator *(double factor, Frequency left) => new Frequency(left.Value * factor, left.Unit);

        #endregion

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(Frequency other) => QuantityHelpers.IsEqual(this, other, ReferenceUnit);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Frequency f && Equals(f);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        #endregion

        /// <inheritdoc />
        public int CompareTo(Frequency other) => QuantityHelpers.Compare(this, other, ReferenceUnit);

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is Frequency))
                throw new ArgumentException($"Object must be of type {nameof(Frequency)}");
            return CompareTo((Frequency) obj);
        }

        /// <inheritdoc />
        public Frequency ConvertUnit(IPhysicalUnit newUnit) => new FrequencyUnitConverter().ConvertUnit(this, newUnit);

        /// <summary> Instanced a new <see cref="Frequency" /> from its time period. </summary>
        /// <param name="period"> The period. </param>
        /// <returns> A <see cref="Frequency" />. </returns>
        public static Frequency FromPeriod(double period) => new Frequency(1 / period);

        /// <inheritdoc />
        public override string ToString() => $"{Value.RoundSignificantFigures(5)} {Unit.Symbol}";
    }
}