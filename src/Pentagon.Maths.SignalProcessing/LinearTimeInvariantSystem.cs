// -----------------------------------------------------------------------
//  <copyright file="LinearTimeInvariantSystem.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using Functions;
    using Quantities;

    public class LinearTimeInvariantSystem : ILinearSystem
    {
        public LinearTimeInvariantSystem(ISystemFunction system)
        {
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
    }
}