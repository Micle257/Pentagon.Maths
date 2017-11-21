namespace Pentagon.Maths {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Polynomial
    {
        readonly string _variableName;
        public IList<double> Coefficients { get; }

        public Polynomial(IEnumerable<double> coefficients, string variableName = "x")
        {
            _variableName = variableName;
            Coefficients = coefficients as IList<double> ?? coefficients.ToList();
        }

        public static Polynomial operator +(Polynomial first, Polynomial second) => Add(first, second);

        public static Polynomial operator -(Polynomial first, Polynomial second) => Substract(first, second);

        public static Polynomial operator *(Polynomial first, Polynomial second) => Multiple(first, second);

        public static Polynomial Add(Polynomial first, Polynomial second)
        {
            if (first._variableName != second._variableName)
                throw new NotSupportedException("The variable names must match.");

            var length = first.Coefficients.Count >= second.Coefficients.Count ? first.Coefficients.Count : second.Coefficients.Count;

            var sum = new List<double>();

            for (int i = 0; i < length; i++)
            {
                var a1 = i.InRange(0, first.Coefficients.Count-1) ? first.Coefficients[i] : 0;
                var a2 = i.InRange(0, second.Coefficients.Count-1) ? second.Coefficients[i] : 0;
                sum.Add(a1 + a2);
            }

            return new Polynomial(sum, first._variableName);
        }

        public static Polynomial Substract(Polynomial first, Polynomial second)
        {
            if (first._variableName != second._variableName)
                throw new NotSupportedException("The variable names must match.");

            var list = new List<double>();
            foreach (var c in second.Coefficients)
            {
                list.Add(-c);
            }
            var negativeSecond = new Polynomial(list, second._variableName);

            return Add(first, negativeSecond);
        }

        public static Polynomial Multiple(Polynomial first, Polynomial second)
        {
            if (first._variableName != second._variableName)
                throw new NotSupportedException("The variable names must match.");

            var result = new double[first.Coefficients.Count + second.Coefficients.Count - 1];
            for (int i = 0; i < result.Length; i++)
            {
                var j = Math.Max(0, i - second.Coefficients.Count + 1);
                var min = Math.Min(i + 1, first.Coefficients.Count);
                for (var k = j; k < min; k++)
                    result[i] += first.Coefficients[k] * second.Coefficients[i-k];
            }
            return new Polynomial(result, first._variableName);
        }
    }
}