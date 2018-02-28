// -----------------------------------------------------------------------
//  <copyright file="Fraction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using Numbers;

    public class NumberFraction<TValue> : Fraction<TValue>
        where TValue : INumber
    {
        /// <inheritdoc />
        public NumberFraction(TValue numerator, TValue denumerator) : base(numerator, denumerator) { }

        /// <inheritdoc />
        public override Fraction<TValue> Add(Fraction<TValue> second)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Fraction<TValue> Substract(Fraction<TValue> second)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Fraction<TValue> Multiple(Fraction<TValue> second)
        {
            var numerator = Numerator.Multiple(second.Numerator);
            var denumerator = Denumerator.Multiple(second.Denumerator);

            return new NumberFraction<TValue>((TValue) numerator, (TValue) denumerator);
        }

        /// <inheritdoc />
        public override Fraction<TValue> Divide(Fraction<TValue> second)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Fraction<TValue> Power(uint exponent)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Fraction<TValue> InTermsOf(Fraction<TValue> second)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class Fraction<TValue>
    {
        public Fraction(TValue numerator, TValue denumerator)
        {
            Numerator = numerator;
            Denumerator = denumerator;
        }

        public TValue Numerator { get; }
        public TValue Denumerator { get; }

        #region Operators

        public static Fraction<TValue> operator +(Fraction<TValue> first, Fraction<TValue> second) => first.Add(second);

        public static Fraction<TValue> operator -(Fraction<TValue> first, Fraction<TValue> second) => first.Substract(second);

        public static Fraction<TValue> operator *(Fraction<TValue> first, Fraction<TValue> second) => first.Multiple(second);

        public static Fraction<TValue> operator /(Fraction<TValue> first, Fraction<TValue> second) => first.Divide(second);

        public static Fraction<TValue> operator ^(Fraction<TValue> first, uint exponent) => first.Power(exponent);

        #endregion

        public abstract Fraction<TValue> Add(Fraction<TValue> second);

        public abstract Fraction<TValue> Substract(Fraction<TValue> second);

        public abstract Fraction<TValue> Multiple(Fraction<TValue> second);

        public abstract Fraction<TValue> Divide(Fraction<TValue> second);

        public abstract Fraction<TValue> Power(uint exponent);

        public abstract Fraction<TValue> InTermsOf(Fraction<TValue> second);
    }
}