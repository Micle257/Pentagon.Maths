namespace Pentagon.Maths.Equations {
    using System;
    using Functions;

    public class CubicFunction : Function
    {
        readonly double _coeffA;
        readonly double _coeffB;
        readonly double _coeffC;
        readonly double _coeffD;

        public CubicFunction(double coeffA, double coeffB, double coeffC, double coeffD)
                : base(x => Math.Pow(x, 3) * coeffA + Math.Pow(x, 2) * coeffB + x * coeffC + coeffD)
        {
            _coeffA = coeffA;
            _coeffB = coeffB;
            _coeffC = coeffC;
            _coeffD = coeffD;
        }

        public QuadraticFunction FirstDerivative => new QuadraticFunction(_coeffA * 3, _coeffB * 2, _coeffC);

        public LinearFunction SecondDerivative => FirstDerivative.FirstDerivative;
    }
}