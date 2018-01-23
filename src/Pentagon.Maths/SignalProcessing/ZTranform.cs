namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Pentagon.Extensions;

    public class ZTranform
    {
        public ZTranform(params double[] coefficients)
        {
            Coefficients = coefficients;
        }

        public ZTranform(IEnumerable<double> coefficients)
        {
            Coefficients = coefficients.ToArray();
        }

        public Polynomial Polynomial => new Polynomial(Coefficients);

        public IList<double> Coefficients { get; }

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            return Sum.ComputeComplex(0, Coefficients.Count, n => Coefficients[n] * Complex.Pow(z, -n));
        }

        /// <inheritdoc />
        public override string ToString() => Polynomial.ToString("z^-");
    }
}