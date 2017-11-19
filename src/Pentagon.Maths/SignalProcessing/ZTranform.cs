namespace Pentagon.Maths.SignalProcessing {
    using System.Collections.Generic;
    using System.Numerics;

    public class ZTranform
    {
        public ZTranform(params double[] coefficients)
        {
            Coefficients = coefficients;
        }

        public IList<double> Coefficients { get; } = new List<double>();

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            return Sum.ComputeComplex(0, Coefficients.Count, n => Coefficients[n] * Complex.Pow(z, -n));
        }
    }
}