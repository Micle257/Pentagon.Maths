// -----------------------------------------------------------------------
//  <copyright file="NumberFraction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Fractions
{
    using System;
    using Numbers;

    public class NumberFraction<TValue> : Fraction<TValue>
            where TValue : INumber
    {
        /// <inheritdoc />
        public NumberFraction(TValue numerator, TValue denumerator) : base(numerator, denumerator) { }

        /// <inheritdoc />
        public override Fraction<TValue> Add(Fraction<TValue> second) => throw new NotImplementedException();

        /// <inheritdoc />
        public override Fraction<TValue> Substract(Fraction<TValue> second) => throw new NotImplementedException();

        /// <inheritdoc />
        public override Fraction<TValue> Multiple(Fraction<TValue> second)
        {
            var numerator = Numerator.Multiple(second.Numerator);
            var denumerator = Denumerator.Multiple(second.Denumerator);

            return new NumberFraction<TValue>((TValue) numerator, (TValue) denumerator);
        }

        /// <inheritdoc />
        public override Fraction<TValue> Divide(Fraction<TValue> second) => throw new NotImplementedException();

        /// <inheritdoc />
        public override Fraction<TValue> Power(uint exponent) => throw new NotImplementedException();

        /// <inheritdoc />
        public override Fraction<TValue> InTermsOf(Fraction<TValue> second) => throw new NotImplementedException();
    }
}