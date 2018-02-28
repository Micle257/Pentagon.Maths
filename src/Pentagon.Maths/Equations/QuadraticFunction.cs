namespace Pentagon.Maths.Equations {
    using System;
    using Functions;

    public class QuadraticFunction : Function
    {
        readonly double _coeffA;
        readonly double _coeffB;
        readonly double _coeffC;

        public QuadraticFunction(double coeffA, double coeffB, double coeffC)
                : base(x => Math.Pow(x, 2) * coeffA + coeffB * x + coeffC )
        {
            _coeffA = coeffA;
            _coeffB = coeffB;
            _coeffC = coeffC;
        }

        public LinearFunction FirstDerivative => new LinearFunction(_coeffA * 2, _coeffB);
    }
}