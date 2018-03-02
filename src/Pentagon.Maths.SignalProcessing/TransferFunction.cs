// -----------------------------------------------------------------------
//  <copyright file="TransferFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Linq.Expressions;
    using Fractions;

    public class TransferFunction : ISystemFunction
    {
        public TransferFunction(SystemTuple coefficients)
        {
            Coefficients = coefficients;

            Numerator = new ZTranform(coefficients.Numerator);
            Denumerator = new ZTranform(coefficients.Denumerator);
        }

        public TransferFunction(ZTranform numerator, ZTranform denumerator)
        {
            Denumerator = denumerator;
            Numerator = numerator;

            Coefficients = new SystemTuple(numerator.Coefficients, denumerator.Coefficients);
        }

        public TransferFunction(double[] numerator, double[] denumeretor) : this(new ZTranform(numerator), new ZTranform(denumeretor)) { }

        public ZTranform Denumerator { get; }

        public ZTranform Numerator { get; }

        /// <inheritdoc />
        public SystemTuple Coefficients { get; }

        #region Operators

        public static TransferFunction operator +(TransferFunction a, TransferFunction b) => Add(a, b);

        public static TransferFunction operator -(TransferFunction a, TransferFunction b) => Substract(a, b);

        public static TransferFunction operator +(TransferFunction a, double value)
        {
            var fraction = a.ToPolynomialFraction().Add(new PolynomialFraction(new[] {value}, new[] {1d}));

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
            var fraction = a.ToPolynomialFraction().Multiple(new PolynomialFraction(new[] {value}, new[] {1d}));

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        public static TransferFunction operator ^(TransferFunction a, uint exp)
        {
            var fraction = a.ToPolynomialFraction().Power(exp);

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        #endregion

        public static TransferFunction FromDifferenceEquationExpression(Expression<Func<RelativeSignal, RelativeSignal, double>> equationFunction) =>
                FromDifferenceEquation(DifferenceEquation.FromExpression(equationFunction));

        public static TransferFunction FromDifferenceEquation(DifferenceEquation equation) => new TransferFunction(equation.Coefficients);

        /// <inheritdoc />
        public override string ToString() => $"H(z) = ({Numerator}) / ({Denumerator})";

        public PolynomialFraction ToPolynomialFraction() => new PolynomialFraction(Numerator.Polynomial, Denumerator.Polynomial);

        static TransferFunction Substract(TransferFunction left, TransferFunction right)
        {
            var a = new PolynomialFraction(left.Numerator.Polynomial, left.Denumerator.Polynomial);
            var b = new PolynomialFraction(right.Numerator.Polynomial, right.Denumerator.Polynomial);
            var mul = a - b;

            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        static TransferFunction Add(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Numerator.Polynomial, tf1.Denumerator.Polynomial);
            var b = new PolynomialFraction(tf2.Numerator.Polynomial, tf2.Denumerator.Polynomial);
            var mul = a + b;

            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        static TransferFunction Multiple(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Numerator.Polynomial, tf1.Denumerator.Polynomial);
            var b = new PolynomialFraction(tf2.Numerator.Polynomial, tf2.Denumerator.Polynomial);
            var mul = a * b;

            return new TransferFunction(new ZTranform(mul.Numerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }
    }
}