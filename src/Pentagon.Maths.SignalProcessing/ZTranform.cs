namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Pentagon.Extensions;

    public static class TransferFunctionExtensions
    {
        public static DifferenceEquation ToDifferenceEquation(this TransferFunction function)
        {
            return DifferenceEquation.FromTransferFunction(function);
        }
    }

    public struct ZTranform
    {
        public ZTranform(Polynomial coefficients)
        {
            Coefficients = coefficients.Coefficients.ToArray();
            Polynomial = coefficients;
        }

        public ZTranform(IEnumerable<double> coefficients)
        {
            Coefficients = coefficients.ToArray();
            Polynomial = new Polynomial(Coefficients);
        }

        public ZTranform(params double[] coefficients)
        {
            Coefficients = coefficients.ToArray();
            Polynomial = new Polynomial(Coefficients);
        }

        public Polynomial Polynomial { get; }

        public IReadOnlyList<double> Coefficients { get; }

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            var tmpThis = this;
            return Sum.ComputeComplex(0, tmpThis.Coefficients.Count, n => tmpThis.Coefficients[n] * Complex.Pow(z, -n));
        }

        /// <inheritdoc />
        public override string ToString() => Polynomial.ToString("z^-");
    }
}