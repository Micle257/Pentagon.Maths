// -----------------------------------------------------------------------
//  <copyright file="QuadraticEquation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Equations
{
    using System;
    using System.Diagnostics;
    using System.Numerics;
    using Functions;
    using Pentagon.Extensions;

    public class QuadraticEquation : Function
    {
        public QuadraticEquation(double a, double b, double c)
        {
            if (a.EqualTo(0))
                throw new ArgumentOutOfRangeException(a.GetType().Name, message: "A coefficient must be non-zero.");

            CoefficientA = a;
            CoefficientB = b;
            CoefficientC = c;

            Discriminant = Math.Pow(CoefficientB, 2) - 4 * CoefficientA * CoefficientC;
            if (Discriminant < 0)
                Type = QuadraticEquationResultType.ImaginaryRoot;
            else
            {
                if (Discriminant > 0)
                    Type = QuadraticEquationResultType.TwoRoots;
                else
                    Type = QuadraticEquationResultType.DoubleRoot;
            }

            ComputeRoots();
        }

        public event EventHandler RootsComputed;
        public double CoefficientA { get; }
        public double CoefficientB { get; }
        public double CoefficientC { get; }
        public MathPoint ExtremePoint => new MathPoint(Derivative.Root, GetValue(Derivative.Root));

        public LinearEquation Derivative => new LinearEquation(CoefficientA * 2, CoefficientB);

        public double Discriminant { get; }
        public QuadraticEquationResultType Type { get; }
        public Complex Root1 { get; private set; }
        public Complex Root2 { get; private set; }
        public TimeSpan ComputeTime { get; private set; }

        public override double GetValue(double x) => Math.Pow(x, 2) * CoefficientA + CoefficientB * x + CoefficientC;

        public override string ToString()
        {
            var b = CoefficientB.EqualTo(0) ? "" : $"{(CoefficientB < 0 ? " - " : " + ")}{Math.Abs(CoefficientB)}x";
            var c = CoefficientC.EqualTo(0) ? "" : $"{(CoefficientC < 0 ? " - " : " + ")}{Math.Abs(CoefficientC)}";
            return $"f(x) = {(CoefficientA < 0 ? "- " : "")}{Math.Abs(CoefficientA)}x^2{b}{c}";
        }

        void ComputeRoots()
        {
            var time = new Stopwatch();
            time.Start();
            switch (Type)
            {
                case QuadraticEquationResultType.TwoRoots:
                    Root1 = (CoefficientB + Math.Sqrt(Discriminant)) / (2 * CoefficientA);
                    Root2 = (CoefficientB - Math.Sqrt(Discriminant)) / (2 * CoefficientA);
                    break;
                case QuadraticEquationResultType.DoubleRoot:
                    Root1 = Root2 = CoefficientB / (2 * CoefficientA);
                    break;
                case QuadraticEquationResultType.ImaginaryRoot:
                    Root1 = new Complex(CoefficientB / (2 * CoefficientA),
                                        Math.Sqrt(Math.Abs(Discriminant)) / (-2 * CoefficientA));
                    Root2 = new Complex(CoefficientB / (2 * CoefficientA),
                                        Math.Sqrt(Math.Abs(Discriminant)) / (2 * CoefficientA));
                    break;
            }

            time.Stop();
            ComputeTime = new TimeSpan(time.ElapsedTicks);
            RootsComputed?.Invoke(this, null);
        }
    }
}