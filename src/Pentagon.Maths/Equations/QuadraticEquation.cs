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
    using System.Threading;
    using System.Threading.Tasks;
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

            _derivative = new LinearEquation(CoefficientA * 2, CoefficientB);
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
                _discriminant = Math.Pow(CoefficientB, 2) - 4 * CoefficientA * CoefficientC;
                if (Discriminant < 0)
                    _resultType = QuadraticEquationResultType.ImaginaryRoot;
                else
                {
                    _resultType = Discriminant > 0 
                                          ? QuadraticEquationResultType.TwoRoots 
                                          : QuadraticEquationResultType.DoubleRoot;
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
        
        public double CoefficientA { get; }
        public double CoefficientB { get; }
        public double CoefficientC { get; }

        public MathPoint ExtremePoint => new MathPoint(_derivative.Root, GetValue(_derivative.Root));

        LinearEquation _derivative;
         QuadraticEquationResultType _resultType;
         Complex _root1;
         Complex _root2;
         TimeSpan _computeTime;
         double _discriminant;

        public double Discriminant
        {
            get
            {
                EnsureComputed(); return _discriminant; }
        }

        public bool IsComputed { get; private set; }
        void EnsureComputed()
        {
            if (!IsComputed)
                throw new EquationNotComputedException();
        }
        public QuadraticEquationResultType ResultType
        {
            get
            {
                EnsureComputed();
                return _resultType;
            }
        }

        public Complex Root1
        {
            get
            {
                EnsureComputed(); return _root1; }
        }

        public Complex Root2
        {
            get
            {
                EnsureComputed(); return _root2; }
        }

        public TimeSpan ComputeTime
        {
            get
            {
                EnsureComputed(); return _computeTime; }
        }

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
            switch (ResultType)
            {
                case QuadraticEquationResultType.TwoRoots:
                    _root1 = (CoefficientB + Math.Sqrt(Discriminant)) / (2 * CoefficientA);
                    _root2 = (CoefficientB - Math.Sqrt(Discriminant)) / (2 * CoefficientA);
                    break;
                case QuadraticEquationResultType.DoubleRoot:
                    _root1 = _root2 = CoefficientB / (2 * CoefficientA);
                    break;
                case QuadraticEquationResultType.ImaginaryRoot:
                    _root1 = new Complex(CoefficientB / (2 * CoefficientA),
                                        Math.Sqrt(Math.Abs(Discriminant)) / (-2 * CoefficientA));
                    _root2 = new Complex(CoefficientB / (2 * CoefficientA),
                                        Math.Sqrt(Math.Abs(Discriminant)) / (2 * CoefficientA));
                    break;
            }

            time.Stop();
            _computeTime = new TimeSpan(time.ElapsedTicks);
        }
    }
}