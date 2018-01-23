// -----------------------------------------------------------------------
//  <copyright file="TransferFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class TransferFunction
    {
        public TransferFunction(ZTranform numerator, ZTranform denumerator)
        {
            Denumerator = denumerator;
            Numerator = numerator;
        }

        public TransferFunction(double[] numerator, double[] denumeretor)
        {
            Numerator = new ZTranform(numerator);
            Denumerator = new ZTranform(denumeretor);
        }

        public ZTranform Denumerator { get; }

        public ZTranform Numerator { get; }

        #region Operators

        public static TransferFunction operator +(TransferFunction a, TransferFunction b) => Add(a, b);

        public static TransferFunction operator -(TransferFunction a, TransferFunction b) => Substract(a, b);

        public static TransferFunction operator +(TransferFunction a, double value)
        {
            var fraction = a.ToPolynomialFraction().Add(new PolynomialFraction(new[] { value }, new[] { 1d }));

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
            var fraction = a.ToPolynomialFraction().Multiple(new PolynomialFraction(new[] { value }, new[] { 1d }));

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        public static TransferFunction operator ^(TransferFunction a, uint exp)
        {
            var fraction = a.ToPolynomialFraction().Power(exp);

            return new TransferFunction(new ZTranform(fraction.Numerator.Coefficients), new ZTranform(fraction.Denumerator.Coefficients));
        }

        #endregion

        public static TransferFunction FromDifferenceEquation(Expression<DifferenceEquationCallback> equationFunction)
        {
            return FromDifferenceEquation(new DifferenceEquation(equationFunction));
        }

        public static TransferFunction FromDifferenceEquation(DifferenceEquation equation)
        {
            var expression = equation.Expression;

            var parameterNames = expression.Parameters.Select(a => a.Name).ToArray();

            if (parameterNames.Length != 2)
                throw new ArgumentException();

            var parameterMap = new Dictionary<string, ValueDirection>
                               {
                                       {parameterNames[0], ValueDirection.Input},
                                       {parameterNames[1], ValueDirection.Output}
                               };

            var body = expression.Body;

            var diff = new DifferenceEquationResolver();

            var mems = diff.ResolveMembers(body);
            var map = diff.Resolve(mems, parameterMap);
            var coeff = diff.GetDefinition(map);

            return new TransferFunction(coeff.Numeretor, coeff.Denumeretor);
        }

        /// <inheritdoc />
        public override string ToString() => $"H(z) = ({Numerator}) / ({Denumerator})";

        public DifferenceEquation GetDifferenceEquation() => GetDifferenceEquation(Numerator.Coefficients, Denumerator.Coefficients);

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

        DifferenceEquation GetDifferenceEquation(IList<double> numerator, IList<double> denumerator)
        {
            return new DifferenceEquation((previousInput, output) =>
                                              (Sum.Compute(0, numerator.Count, n => numerator[n] * previousInput[-n])
                                               - Sum.Compute(1, denumerator.Count, n => denumerator[n] * output[-n])) / denumerator[0]);
        }
    }
}