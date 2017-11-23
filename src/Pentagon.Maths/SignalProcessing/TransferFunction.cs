// -----------------------------------------------------------------------
//  <copyright file="TransferFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System.Collections.Generic;

    public class TransferFunction
    {
        public TransferFunction(ZTranform numerator, ZTranform denumerator)
        {
            Input = denumerator;
            Output = numerator;
        }

        public ZTranform Input { get; }
        public ZTranform Output { get; }

        #region Operators

        public static TransferFunction operator +(TransferFunction a, TransferFunction b) => Add(a, b);

        public static TransferFunction operator -(TransferFunction a, TransferFunction b) => Substract(a, b);

        static TransferFunction Substract(TransferFunction left, TransferFunction right)
        {
            var a = new PolynomialFraction(left.Output.Polynomial, left.Input.Polynomial);
            var b = new PolynomialFraction(right.Output.Polynomial, right.Input.Polynomial);
            var mul = a- b;
            
            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        public static TransferFunction operator +(TransferFunction a, double value)
        {
            var fraction = a.ToPolynomialFraction().Add(new PolynomialFraction(new[] {value}, new[] {1d}, a.Input.Polynomial.VariableName));

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        public static TransferFunction operator *(TransferFunction a, TransferFunction b) => Multiple(a, b);

        public static TransferFunction operator /(TransferFunction a, TransferFunction b)
        {
            var f1 = a.ToPolynomialFraction();
            var f2 = b.ToPolynomialFraction();
            var result = f1 / f2;

            return new TransferFunction(new ZTranform(result.Numerator.Coefficients), new ZTranform(result.Denumerator.Coefficients));
        }

        public static TransferFunction operator *(TransferFunction a, double value)
        {
            var fraction = a.ToPolynomialFraction().Multiple(new PolynomialFraction(new[] {value}, new[] {1d}, a.Input.Polynomial.VariableName));

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        public static TransferFunction operator ^(TransferFunction a, uint exp)
        {
            var fraction = a.ToPolynomialFraction().Power(exp);

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        #endregion

        /// <inheritdoc />
        public override string ToString() => $"H(z) = ({Output}) / ({Input})";

        public DifferenceEquation GetDifferenceEquation() => GetDifferenceEquation(Output.Coefficients, Input.Coefficients);

        public PolynomialFraction ToPolynomialFraction() => new PolynomialFraction(Output.Polynomial, Input.Polynomial);

        static TransferFunction Add(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Output.Polynomial, tf1.Input.Polynomial);
            var b = new PolynomialFraction(tf2.Output.Polynomial, tf2.Input.Polynomial);
            var mul = a + b;

            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        static TransferFunction Multiple(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Output.Polynomial, tf1.Input.Polynomial);
            var b = new PolynomialFraction(tf2.Output.Polynomial, tf2.Input.Polynomial);
            var mul = a * b;

            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        DifferenceEquation GetDifferenceEquation(IList<double> numerator, IList<double> denumerator)
        {
            return new DifferenceEquation((input, previousInput, output) =>
                                              (input
                                               + Sum.Compute(0, numerator.Count, n => numerator[n] * previousInput[-n])
                                               - Sum.Compute(1, denumerator.Count, n => denumerator[n] * output[-n])) / denumerator[0]);
        }
    }
}