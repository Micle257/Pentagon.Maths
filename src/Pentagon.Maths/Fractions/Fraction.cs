// -----------------------------------------------------------------------
//  <copyright file="Fraction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Fractions
{
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