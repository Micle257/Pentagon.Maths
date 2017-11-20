// -----------------------------------------------------------------------
//  <copyright file="TransferFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransferFunction : ISystemDefinition
    {
        public TransferFunction(ZTranform numerator, ZTranform denumerator)
        {
            Input = denumerator;
            Output = numerator;
            DifferenceEquation = GetDifferenceEquation(numerator.Coefficients, denumerator.Coefficients);
        }

        public ZTranform Input { get; }
        public ZTranform Output { get; }

        public DifferenceEquation DifferenceEquation { get; }

        #region Operators

        public static TransferFunction operator +(TransferFunction a, TransferFunction b) => Add(a, b);

         static TransferFunction Add(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Output.Polynomial, tf1.Input.Polynomial);
            var b = new PolynomialFraction(tf2.Output.Polynomial, tf2.Input.Polynomial);
            var mul = a + b;

            return new TransferFunction(new ZTranform(mul.Denumerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        public static TransferFunction operator *(TransferFunction a, TransferFunction b) => Multiple(a, b);

        static TransferFunction Multiple(TransferFunction tf1, TransferFunction tf2)
        {
            var a = new PolynomialFraction(tf1.Output.Polynomial, tf1.Input.Polynomial);
            var b = new PolynomialFraction(tf2.Output.Polynomial, tf2.Input.Polynomial);
            var mul = a * b;

            return new TransferFunction(new ZTranform(mul.Denumerator.Coefficients), new ZTranform(mul.Denumerator.Coefficients));
        }

        public static TransferFunction operator *(TransferFunction a, double value)
        {
            var s = a.Output.Coefficients;
            for (var index = 0; index < s.Count; index++)
                s[index] *= value;
            return new TransferFunction(new ZTranform(s.ToArray()), a.Input);
        }

        public static TransferFunction operator ^(TransferFunction a, int exp)
        {
            if (exp > 1)
            {
                for (var i = 1; i < exp; i++)
                    a *= a;
                return a;
            }
            return a;
        }

        #endregion

        DifferenceEquation GetDifferenceEquation(IList<double> numerator, IList<double> denumerator)
        {
            return new DifferenceEquation((input, previousInput, output) =>
                                           (input
                                              + Sum.Compute(0, numerator.Count, n => numerator[n] * previousInput[-n])
                                              - Sum.Compute(1, denumerator.Count, n => denumerator[n] * output[-n])) / denumerator[0]);
        }

        public TransferFunction AddOne()
        {
            var input = Input.Coefficients;
            var output = Output.Coefficients;
            var newOutput = Input.Polynomial + Output.Polynomial;
            return new TransferFunction(new ZTranform(newOutput.Coefficients), Input);
        }

        public TransferFunction Invert() => new TransferFunction(Input, Output);

        public double EvaluateNext(double x) => DifferenceEquation.EvaluateNext(x);

        /// <inheritdoc />
        public override string ToString()
        {
            return $"H(z) = ({Output}) / ({Input})";
        }
    }
}