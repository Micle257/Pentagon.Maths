// -----------------------------------------------------------------------
//  <copyright file="LinearEquation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Equations
{
    using System;
    using Pentagon.Extensions;

    public class LinearEquation
    {
        public LinearEquation(double a, double b)
        {
            if (a.EqualTo(0))
                throw new ArgumentOutOfRangeException(a.GetType().Name, "A coefficient must be non-zero.");

            CoefficientA = a;
            CoefficientB = b;
            Function = new LinearFunction(a, b);
        }

        public double CoefficientA { get; }
        public double CoefficientB { get; }
        public double Root => -CoefficientB / CoefficientA;

        public LinearFunction Function { get; }

        public override string ToString()
        {
            var b = CoefficientB.EqualTo(0) ? "" : $"{(CoefficientB < 0 ? " - " : " + ")}{Math.Abs(CoefficientB)}";
            return $"{(CoefficientA < 0 ? "- " : "")}{Math.Abs(CoefficientA)}x{b}";
        }

        public double GetValue(double x) => Function.GetValue(x);

        public string ToFullString()
        {
            var b = CoefficientB.EqualTo(0) ? "" : $"{(CoefficientB < 0 ? " - " : " + ")}{Math.Abs(CoefficientB)}";
            return $"f(x) = {(CoefficientA < 0 ? "- " : "")}{Math.Abs(CoefficientA)}x{b}";
        }
    }
}