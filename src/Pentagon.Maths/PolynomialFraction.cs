// -----------------------------------------------------------------------
//  <copyright file="PolynomialFraction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System.Collections.Generic;

    public class PolynomialFraction : Fraction<Polynomial>
    {
        public PolynomialFraction(Polynomial numerator, Polynomial denumerator) : base(numerator, denumerator) { }

        public PolynomialFraction(IEnumerable<double> numeratorCoefficients, IEnumerable<double> denumeratorCoefficients, string variableName = "x")
               : this(new Polynomial(numeratorCoefficients),new Polynomial(denumeratorCoefficients) )
        {
        }

        public override Fraction<Polynomial> Add(Fraction<Polynomial> second)
        {
            var left = InTermsOf(second);
            var right = second.InTermsOf(this);

            return new PolynomialFraction(left.Numerator + right.Numerator, left.Denumerator);
        }

        public override Fraction<Polynomial> Substract(Fraction<Polynomial> second)
        {
            return Add(new PolynomialFraction(-second.Numerator, second.Denumerator));
        }

        public override Fraction<Polynomial> Multiple(Fraction<Polynomial> second) => new PolynomialFraction(Numerator * second.Numerator, Denumerator * second.Denumerator);

        public override Fraction<Polynomial> Divide(Fraction<Polynomial> second)
        {
            return this.Multiple(new PolynomialFraction(second.Denumerator, second.Numerator));
        }

        public override Fraction<Polynomial> Power(uint exponent) => new PolynomialFraction(Numerator.Power(exponent), Denumerator.Power(exponent));

        public override Fraction<Polynomial> InTermsOf(Fraction<Polynomial> second) => new PolynomialFraction(Numerator * second.Denumerator, Denumerator * second.Denumerator);
    }
}