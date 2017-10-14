// -----------------------------------------------------------------------
//  <copyright file="CubicEquation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Equations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using Functions;
    using Pentagon.Extensions;

    public class CubicEquation : Function
    {
        public CubicEquation(double a, double b, double c, double d)
        {
            if (a.EqualTo(0))
                throw new ArgumentOutOfRangeException(a.GetType().Name, "A coefficient must be non-zero.");

            CoefficientA = a;
            CoefficientB = b;
            CoefficientC = c;
            CoefficientD = d;

            RedCoifP = -Math.Pow(CoefficientB, 2) / (3 * Math.Pow(CoefficientA, 2)) + CoefficientC / CoefficientA;
            RedCoifQ = 2 * Math.Pow(CoefficientB, 3) / (27 * Math.Pow(CoefficientA, 3)) -
                       CoefficientB * CoefficientC / (3 * Math.Pow(CoefficientA, 2)) + CoefficientD / CoefficientA;
            Discriminant = Math.Pow(RedCoifQ / 2, 2) + Math.Pow(RedCoifP / 3, 3);
            if (Discriminant < 0)
                Type = CubicEquationResultType.ThreeReal;
            else
            {
                if (Discriminant > 0)
                    Type = CubicEquationResultType.OneRealTwoComplex;
                else
                {
                    if (Discriminant.EqualTo(0) && RedCoifP.EqualTo(RedCoifQ))
                        Type = CubicEquationResultType.OneReal;
                    else
                        Type = CubicEquationResultType.TwoReal;
                }
            }
            ComputeRoots();
        }

        public event EventHandler RootsComputed;
        public double CoefficientA { get; }
        public double CoefficientB { get; }
        public double CoefficientC { get; }
        public double CoefficientD { get; }
        public QuadraticEquation FirstDerivative => new QuadraticEquation(CoefficientA * 3, CoefficientB * 2, CoefficientC);

        public LinearEquation SecondDerivative => FirstDerivative.Derivative;

        public CubicEquationResultType Type { get; }
        public double Discriminant { get; }
        public Complex Root1 { get; private set; }
        public Complex Root2 { get; private set; }
        public Complex Root3 { get; private set; }
        public TimeSpan ComputeTime { get; private set; }

        double RedCoifP { get; }
        double RedCoifQ { get; }

        public override double GetValue(double x) => Math.Pow(x, 3) * CoefficientA + Math.Pow(x, 2) * CoefficientB + x * CoefficientC + CoefficientD;

        public override string ToString()
        {
            var b = CoefficientB.EqualTo(0) ? "" : $"{(CoefficientB < 0 ? " - " : " + ")}{Math.Abs(CoefficientB)}x^2";
            var c = CoefficientC.EqualTo(0) ? "" : $"{(CoefficientC < 0 ? " - " : " + ")}{Math.Abs(CoefficientC)}x";
            var d = CoefficientD.EqualTo(0) ? "" : $"{(CoefficientD < 0 ? " - " : " + ")}{Math.Abs(CoefficientD)}";
            return $"f(x) = {(CoefficientA < 0 ? "- " : "")}{Math.Abs(CoefficientA)}x^3{b}{c}{d}";
        }

        public IEnumerable<MathPoint> GetExtremePoints()
        {
            var list = new List<MathPoint>();
            switch (Type)
            {
                case CubicEquationResultType.OneRealTwoComplex:
                case CubicEquationResultType.TwoReal:
                case CubicEquationResultType.ThreeReal:
                    list.Add(new MathPoint(FirstDerivative.Root1.Real, GetValue(FirstDerivative.Root1.Real)));
                    list.Add(new MathPoint(FirstDerivative.Root2.Real, GetValue(FirstDerivative.Root2.Real)));
                    break;
            }
            if (list.Any(a => a == GetInflectionPoint()))
                list.Clear();
            return list;
        }

        public MathPoint GetInflectionPoint()
        {
            switch (Type)
            {
                case CubicEquationResultType.OneRealTwoComplex:
                case CubicEquationResultType.TwoReal:
                case CubicEquationResultType.ThreeReal:
                    return new MathPoint(SecondDerivative.Root, GetValue(SecondDerivative.Root));
            }
            return default(MathPoint);
        }

        void ComputeRoots()
        {
            var time = new Stopwatch();
            time.Start();
            switch (Type)
            {
                case CubicEquationResultType.TwoReal:
                case CubicEquationResultType.ThreeReal:
                    var i = Math.Sqrt(Math.Pow(RedCoifQ, 2) / 4 - Discriminant);
                    var k = Math.Acos(-(RedCoifQ / (2 * i)));
                    var m = Math.Cos(k / 3);
                    var n = Math.Sqrt(3) * Math.Sin(k / 3);
                    var p = -(CoefficientB / (3 * CoefficientA));
                    Root1 = 2 * i.Cbrt() * m + p;
                    Root2 = -i.Cbrt() * (m + n) + p;
                    Root3 = -i.Cbrt() * (m - n) + p;
                    break;
                case CubicEquationResultType.OneRealTwoComplex:
                    var r = -(RedCoifQ / 2) + Math.Sqrt(Discriminant);
                    var s = r.Cbrt();
                    var t = -(RedCoifQ / 2) - Math.Sqrt(Discriminant);
                    var u = t.Cbrt();
                    Root1 = Math.Round(s + u - CoefficientB / (3 * CoefficientA), 10);
                    Root2 = new Complex(-(s + u) / 2 - CoefficientB / (3 * CoefficientA), (s - u) * Math.Sqrt(3) / 2);
                    Root3 = new Complex(-(s + u) / 2 - CoefficientB / (3 * CoefficientA), -(s - u) * Math.Sqrt(3) / 2);
                    break;
                case CubicEquationResultType.OneReal:
                    Root1 = Root2 = Root3 = -(CoefficientD / CoefficientA).Cbrt();
                    break;
            }
            time.Stop();
            ComputeTime = new TimeSpan(time.ElapsedTicks);
            RootsComputed?.Invoke(this, null);
        }
    }
}