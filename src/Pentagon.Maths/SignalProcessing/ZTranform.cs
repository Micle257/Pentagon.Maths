namespace Pentagon.Maths.SignalProcessing
{
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

        public Polynomial Polynomial => new Polynomial(Coefficients, "z-");

        public IList<double> Coefficients { get; }

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            return Sum.ComputeComplex(0, Coefficients.Count, n => Coefficients[n] * Complex.Pow(z, -n));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = new StringBuilder();

            for (int i = 0; i < Coefficients.Count; i++)
            {
                if (Coefficients[i].EqualTo(0, 5))
                    continue;

                    result.Append($"{Coefficients[i].SignificantFigures(3)}");

                if (i != 0)
                    result.Append($"z^-{i}");
            }

            return result.ToString();
        }
    }
}