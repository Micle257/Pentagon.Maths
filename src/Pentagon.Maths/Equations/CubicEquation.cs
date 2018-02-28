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
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Pentagon.Extensions;

    public class CubicEquation
    {
        double _redCoifP;
        double _redCoifQ;
        Complex _root1;
        Complex _root2;
        Complex _root3;
        TimeSpan _computeTime;
        double _discriminant;
        CubicEquationResultType _resultType;
        readonly QuadraticEquation _firstDerivative;
        readonly LinearEquation _secondDerivative;

        public CubicEquation(double a, double b, double c, double d)
        {
            if (a.EqualTo(0))
                throw new ArgumentOutOfRangeException(a.GetType().Name, message: "A coefficient must be non-zero.");

            CoefficientA = a;
            CoefficientB = b;
            CoefficientC = c;
            CoefficientD = d;

            Function = new CubicFunction(a, b, c, d);
            _firstDerivative = new QuadraticEquation(3 * CoefficientA, 2 * CoefficientB, CoefficientC);
            _secondDerivative = new LinearEquation(6 * CoefficientA * CoefficientA, 2 * CoefficientB * CoefficientB);
        }

        public double CoefficientA { get; }

        public double CoefficientB { get; }

        public double CoefficientC { get; }

        public double CoefficientD { get; }

        public CubicEquationResultType ResultType
        {
            get
            {
                EnsureComputed();
                return _resultType;
            }
        }

        public double Discriminant
        {
            get
            {
                EnsureComputed();
                return _discriminant;
            }
        }

        public Complex Root1
        {
            get
            {
                EnsureComputed();
                return _root1;
            }
        }

        public Complex Root2
        {
            get
            {
                EnsureComputed();
                return _root2;
            }
        }

        public Complex Root3
        {
            get
            {
                EnsureComputed();
                return _root3;
            }
        }

        public TimeSpan ComputeTime
        {
            get
            {
                EnsureComputed();
                return _computeTime;
            }
        }

        public CubicFunction Function { get; }

        public bool IsComputed { get; private set; }

        public override string ToString()
        {
            var b = CoefficientB.EqualTo(0) ? "" : $"{(CoefficientB < 0 ? " - " : " + ")}{Math.Abs(CoefficientB)}x^2";
            var c = CoefficientC.EqualTo(0) ? "" : $"{(CoefficientC < 0 ? " - " : " + ")}{Math.Abs(CoefficientC)}x";
            var d = CoefficientD.EqualTo(0) ? "" : $"{(CoefficientD < 0 ? " - " : " + ")}{Math.Abs(CoefficientD)}";
            return $"f(x) = {(CoefficientA < 0 ? "- " : "")}{Math.Abs(CoefficientA)}x^3{b}{c}{d}";
        }

        public Task EnsureComputedAsync(CancellationToken cancellationToken = default)
        {
            if (!IsComputed)
                return ComputeRootsAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public Task ComputeRootsAsync(CancellationToken cancellationToken = default)
        {
            if (IsComputed)
                return Task.CompletedTask;

            cancellationToken.ThrowIfCancellationRequested();

            var source = new TaskCompletionSource<bool>();

            try
            {
                _redCoifP = -Math.Pow(CoefficientB, 2) / (3 * Math.Pow(CoefficientA, 2)) + CoefficientC / CoefficientA;
                _redCoifQ = 2 * Math.Pow(CoefficientB, 3) / (27 * Math.Pow(CoefficientA, 3)) -
                            CoefficientB * CoefficientC / (3 * Math.Pow(CoefficientA, 2)) + CoefficientD / CoefficientA;

                _discriminant = Math.Pow(_redCoifQ / 2, 2) + Math.Pow(_redCoifP / 3, 3);

                if (Discriminant < 0)
                    _resultType = CubicEquationResultType.ThreeReal;
                else
                {
                    if (Discriminant > 0)
                        _resultType = CubicEquationResultType.OneRealTwoComplex;
                    else
                    {
                        if (Discriminant.EqualTo(0) && _redCoifP.EqualTo(_redCoifQ))
                            _resultType = CubicEquationResultType.OneReal;
                        else
                            _resultType = CubicEquationResultType.TwoReal;
                    }
                }

                ComputeRoots();

                IsComputed = true;
                source.SetResult(true);
            }
            catch (Exception e)
            {
                source.SetException(e);
            }

            return source.Task;
        }

        public IEnumerable<MathPoint> GetExtremePoints()
        {
            _firstDerivative.EnsureComputedAsync().Wait();

            var list = new List<MathPoint>();
            list.Add(new MathPoint(_firstDerivative.Root1.Real, Function.GetValue(_firstDerivative.Root1.Real)));
            list.Add(new MathPoint(_firstDerivative.Root2.Real, Function.GetValue(_firstDerivative.Root2.Real)));

            if (list.Any(a => a == GetInflectionPoint()))
                list.Clear();
            return list;
        }

        public MathPoint GetInflectionPoint() => new MathPoint(_secondDerivative.Root, Function.GetValue(_secondDerivative.Root));

        void EnsureComputed()
        {
            if (!IsComputed)
                throw new EquationNotComputedException();
        }

        void ComputeRoots()
        {
            var time = new Stopwatch();
            time.Start();
            switch (ResultType)
            {
                case CubicEquationResultType.TwoReal:
                case CubicEquationResultType.ThreeReal:
                    var i = Math.Sqrt(Math.Pow(_redCoifQ, 2) / 4 - Discriminant);
                    var k = Math.Acos(-(_redCoifQ / (2 * i)));
                    var m = Math.Cos(k / 3);
                    var n = Math.Sqrt(3) * Math.Sin(k / 3);
                    var p = -(CoefficientB / (3 * CoefficientA));
                    _root1 = 2 * i.Cbrt() * m + p;
                    _root2 = -i.Cbrt() * (m + n) + p;
                    _root3 = -i.Cbrt() * (m - n) + p;
                    break;
                case CubicEquationResultType.OneRealTwoComplex:
                    var r = -(_redCoifQ / 2) + Math.Sqrt(Discriminant);
                    var s = r.Cbrt();
                    var t = -(_redCoifQ / 2) - Math.Sqrt(Discriminant);
                    var u = t.Cbrt();
                    _root1 = Math.Round(s + u - CoefficientB / (3 * CoefficientA), 10);
                    _root2 = new Complex(-(s + u) / 2 - CoefficientB / (3 * CoefficientA), (s - u) * Math.Sqrt(3) / 2);
                    _root3 = new Complex(-(s + u) / 2 - CoefficientB / (3 * CoefficientA), -(s - u) * Math.Sqrt(3) / 2);
                    break;
                case CubicEquationResultType.OneReal:
                    _root1 = _root2 = _root3 = -(CoefficientD / CoefficientA).Cbrt();
                    break;
            }

            time.Stop();
            _computeTime = new TimeSpan(time.ElapsedTicks);
        }
    }
}