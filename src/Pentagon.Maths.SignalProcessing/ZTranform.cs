// -----------------------------------------------------------------------
//  <copyright file="ZTranform.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

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

        /// <inheritdoc />
        public override string ToString() => Polynomial.ToString(variableName: "z^-");

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            var tmpThis = this;
            return SumHelper.ComputeComplex(0, tmpThis.Coefficients.Count, n => new Complex(tmpThis.Coefficients[n], 0) * Complex.Pow(z, -n));
        }
    }
}