// -----------------------------------------------------------------------
//  <copyright file="LinearTimeInvariantSystem.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Numerics;
    using Functions;
    using JetBrains.Annotations;
    using Quantities;

    public class LinearTimeInvariantSystem : ILinearSystem
    {
        readonly ISystemFunction _originalFunction;

        public LinearTimeInvariantSystem([NotNull] ISystemFunction system)
        {
            _originalFunction = system;
            TransferFunction = system as TransferFunction ?? new TransferFunction(system.Coefficients);
            DifferenceEquation = system as DifferenceEquation ?? new DifferenceEquation(system.Coefficients);
        }

        public TransferFunction TransferFunction { get; }
        public DifferenceEquation DifferenceEquation { get; }

        public IDiscreteFunction GetImpulseResponse(Frequency samplingFrequency)
        {
            var imp = InfiniteDiscreteFunction.ImpulseFunction(samplingFrequency);
            Func<int, double> func = i =>
                                     {
                                         if (i < 0)
                                             return 0;

                                         var input = imp.EvaluateSample(i);

                                         var d = ProcessSample(input);

                                         return d;
                                     };
            return new InfiniteDiscreteFunction(func, samplingFrequency);
        }

        public double ProcessSample(double sample) => DifferenceEquation.EvaluateNext(sample);

        public Complex EvaluateFrequency(Frequency frequency, Frequency samplingFrequency) => EvaluateFrequency(ToComplexFrequency(frequency, samplingFrequency));

        public Complex EvaluateFrequency(Complex complexFrequency)
        {
            var num = SumHelper.ComputeComplex(0,
                                               _originalFunction.Coefficients.Order,
                                               n => new Complex(_originalFunction.Coefficients.Numerator[n], 0) * Complex.Pow(complexFrequency, -n));
            var den = SumHelper.ComputeComplex(0,
                                               _originalFunction.Coefficients.Order,
                                               n => new Complex(_originalFunction.Coefficients.Denumerator[n], 0) * Complex.Pow(complexFrequency, -n));

            return num / den;
        }

        public double EvaluatePhase(Frequency frequency, Frequency samplingFrequency) => EvaluatePhase(ToComplexFrequency(frequency, samplingFrequency));

        public double EvaluatePhase(Complex complexFrequency)
        {
            var arg = Math.Atan2(complexFrequency.Imaginary, complexFrequency.Real);

            if (arg < 0)
                arg = arg + 2 * Math.PI;

            return arg;
        }

        public double EvaluatePhaseDelay(Frequency frequency, Frequency samplingFrequency)
        {
            var omega = 2 * Math.PI * frequency.Value * samplingFrequency.Period;

            var phase = EvaluatePhase(frequency, samplingFrequency);

            return -phase / omega;
        }

        public double EvaluateGroupDelay(Frequency frequency, Frequency samplingFrequency)
        {
            var df = 0.1;
            var upFrequency = (Frequency) (frequency + df);
            var downFrequency = (Frequency) (frequency - df);

            var upOmega = 2 * Math.PI * upFrequency.Value * samplingFrequency.Period;
            var downOmega = 2 * Math.PI * downFrequency.Value * samplingFrequency.Period;

            var upPhase = EvaluatePhase(upFrequency, samplingFrequency);
            var downPhase = EvaluatePhase(downFrequency, samplingFrequency);

            return (upOmega * upPhase - downOmega * downPhase) / (upOmega - downOmega);
        }

        Complex ToComplexFrequency(Frequency frequency, Frequency samplingFrequency)
        {
            var omega = 2 * Math.PI * frequency.Value * samplingFrequency.Period;
            return Complex.FromPolarCoordinates(1, omega);
        }
    }
}