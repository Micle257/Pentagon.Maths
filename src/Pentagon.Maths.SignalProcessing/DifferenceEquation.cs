// -----------------------------------------------------------------------
//  <copyright file="DifferenceEquation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public struct SystemTuple
    {
        public SystemTuple(IEnumerable<double> numeratorCoefficients, IEnumerable<double> denumeratorCoefficients)
        {
            var num = numeratorCoefficients as double[] ?? numeratorCoefficients?.ToArray();
            var den = denumeratorCoefficients as double[] ?? denumeratorCoefficients?.ToArray();

            if (num == null || num.Length == 0)
                num = new[] { 1d };

            if (den == null || den.Length == 0)
                den = new[] { 1d };

            if (den.Length != num.Length)
            {
                if (num.Length < den.Length)
                    Array.Resize(ref num, den.Length);
                else
                    Array.Resize(ref den, num.Length);
            }

            Numerator = num;
            Denumerator = den;

            Order = num.Length;
        }

        public double[] Numerator { get; }

        public double[] Denumerator { get; }

        public int Order { get; }
    }

    public interface ISystemFunction
    {
        SystemTuple Coefficients { get; }
    }

    public class DifferenceEquation : ISystemFunction
    {
        RelativeSignal _inputSignal;
        RelativeSignal _outputSignal;

        public DifferenceEquation(IEnumerable<double> numeretorCoefficients, IEnumerable<double> denumeratorCoefficients) : this(new SystemTuple(numeretorCoefficients, denumeratorCoefficients)) { }

        public DifferenceEquation(SystemTuple tuple)
        {
            if (tuple.Order == 0)
                throw new ArgumentException(message: "The coefficients are not specified.");

            Coefficients = tuple;

            _inputSignal = new RelativeSignal(tuple.Order);
            _outputSignal = new RelativeSignal(tuple.Order);
        }

        public SystemTuple Coefficients { get; }

        public static DifferenceEquation FromTransferFunction(TransferFunction function)
        {
            var builder = new DifferenceEquationBuilder();

            return builder.Build(function);
        }

        public static DifferenceEquation FromExpression(Expression<Func<RelativeSignal, RelativeSignal, double>> function)
        {
            var resolver = new DifferenceEquationResolver();

            var cs = resolver.GetCoefficients(function);

            return new DifferenceEquation(cs);
        }

        public DifferenceEquation CopyInitial()
        {
            return new DifferenceEquation(Coefficients);
        }
        
        public double LastValue { get; private set; }

        public double EvaluateNext(double x)
        {
            _inputSignal.AddSample(x);
            _outputSignal.AddSample(_outputSignal[0]);

            double inSum = 0d, outSum = 0d;
            for (var i = 0; i < Coefficients.Order; i++)
            {
                inSum += Coefficients.Numerator[i] * _inputSignal[-i];

                if (i != 0)
                    outSum += Coefficients.Denumerator[i] * _outputSignal[-i];
            }

            LastValue = inSum - outSum;

            _outputSignal.SetLastSample(LastValue);

            return LastValue;
        }

        public IEnumerable<double> EvaluateSignal(IEnumerable<double> samples)
        {
            foreach (var sample in samples)
                yield return EvaluateNext(sample);
        }
    }
}