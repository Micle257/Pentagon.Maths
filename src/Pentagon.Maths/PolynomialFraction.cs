// -----------------------------------------------------------------------
//  <copyright file="PolynomialFraction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    public class PolynomialFraction : Fraction<Polynomial>
    {
        public PolynomialFraction(Polynomial numerator, Polynomial denumerator) : base(numerator, denumerator) { }

        public override Fraction<Polynomial> Add(Fraction<Polynomial> second)
        {
            var left = InTermsOf(second);
            var right = second.InTermsOf(this);

            return new PolynomialFraction(left.Numerator + right.Numerator, left.Denumerator);
        }

        public override Fraction<Polynomial> Multiple(Fraction<Polynomial> second) => new PolynomialFraction(Numerator * second.Numerator, Denumerator * second.Denumerator);

        public override Fraction<Polynomial> InTermsOf(Fraction<Polynomial> second) => new PolynomialFraction(Numerator * second.Denumerator, Denumerator * second.Denumerator);
    }
}