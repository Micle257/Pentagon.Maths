// -----------------------------------------------------------------------
//  <copyright file="Polynomial.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Pentagon.Extensions;

    public struct Polynomial
    {
        public Polynomial(IEnumerable<double> coefficients)
        {
            // VariableName = variableName;
            Coefficients = coefficients as IList<double> ?? coefficients.ToList();
        }

        public IList<double> Coefficients { get; }

        #region Operators

        public static Polynomial operator +(Polynomial first, Polynomial second) => first.Add(second);

        public static Polynomial operator -(Polynomial first) => first.Invert();

        public static Polynomial operator -(Polynomial first, Polynomial second) => first.Substract(second);

        public static Polynomial operator *(Polynomial first, Polynomial second) => first.Multiple(second);

        public static Polynomial operator *(Polynomial polynomial, double factor) => polynomial.MultipleByFactor(factor);

        public static Polynomial operator *(double factor, Polynomial polynomial) => polynomial.MultipleByFactor(factor);

        public static Polynomial operator ^(Polynomial first, uint exponent) => first.Power(exponent);

        #endregion

        /// <inheritdoc />
        public override string ToString() => ToString(variableName: "x");

        /// <inheritdoc />
        public string ToString(string variableName)
        {
            var result = new StringBuilder();

            for (var i = 0; i < Coefficients.Count; i++)
            {
                if (Math.Abs(Coefficients[i]) < 0.00001)
                    continue;

                if (Coefficients[i] < 0)
                    result.Append($"{Coefficients[i].RoundSignificantFigures(7)}");
                else
                {
                    if (i != 0)
                        result.Append($"+");

                    result.Append($"{Coefficients[i].RoundSignificantFigures(7)}");
                }

                if (i != 0)
                    result.Append($"{variableName}^{i}");
            }

            return result.ToString();
        }

        public Polynomial Multiple(Polynomial second)
        {
            var first = this;

            //  if (first.VariableName != second.VariableName)
            //      throw new NotSupportedException(message: "The variable names must match.");

            var result = new double[first.Coefficients.Count + second.Coefficients.Count - 1];
            for (var i = 0; i < result.Length; i++)
            {
                var j = Math.Max(0, i - second.Coefficients.Count + 1);
                var min = Math.Min(i + 1, first.Coefficients.Count);
                for (var k = j; k < min; k++)
                    result[i] += first.Coefficients[k] * second.Coefficients[i - k];
            }

            return new Polynomial(result);
        }

        public Polynomial Add(Polynomial second)
        {
            var first = this;

            //   if (first.VariableName != second.VariableName)
            //       throw new NotSupportedException(message: "The variable names must match.");

            var length = first.Coefficients.Count >= second.Coefficients.Count ? first.Coefficients.Count : second.Coefficients.Count;

            var sum = new List<double>();

            for (var i = 0; i < length; i++)
            {
                var a1 = i.InRange(0, first.Coefficients.Count - 1) ? first.Coefficients[i] : 0;
                var a2 = i.InRange(0, second.Coefficients.Count - 1) ? second.Coefficients[i] : 0;
                sum.Add(a1 + a2);
            }

            return new Polynomial(sum);
        }

        public Polynomial Substract(Polynomial second)
        {
            var first = this;

            // if (first.VariableName != second.VariableName)
            //     throw new NotSupportedException(message: "The variable names must match.");

            var list = new List<double>();
            foreach (var c in second.Coefficients)
                list.Add(-c);
            var negativeSecond = new Polynomial(list);

            return first.Add(negativeSecond);
        }

        public Polynomial Power(uint exponent)
        {
            if (exponent == 0)
                return new Polynomial(new[] {1d});

            if (exponent == 1)
                return this;

            var basis = this;
            var result = basis;
            for (var i = 1; i < exponent; i++)
                result = basis * result;
            return result;
        }

        Polynomial Invert() => new Polynomial(new[] {0d}) - this;

        Polynomial MultipleByFactor(double factor)
        {
            var coeffs = Coefficients.Select(a => a * factor);

            return new Polynomial(coeffs);
        }

        // public string VariableName { get; }
    }
}