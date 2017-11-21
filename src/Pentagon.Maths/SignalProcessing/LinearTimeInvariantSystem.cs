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

    public class LinearTimeInvariantSystem
    {
        public TransferFunction TransferFunction { get; }
        public DifferenceEquation DifferenceEquation { get; }

        public LinearTimeInvariantSystem(TransferFunction transferFunction)
        {
            TransferFunction = transferFunction;
            DifferenceEquation = TransferFunction.GetDifferenceEquation();
        }
        
        public IDiscreteFunction GetImpulseResponse(Frequency samplingFrequency)
        {
            var imp = InfiniteDiscreteFunction.ImpulseFunction(samplingFrequency);
            Func<int, double> func = i =>
                                     {
                                         if (i < 0)
                                             return 0;
                                         var d = 0d;
                                         for (var j = 0; j < i + 1; j++)
                                           d= DifferenceEquation.EvaluateNext(imp.EvaluateSample(j));
                                         return d;
                                     };
            return new InfiniteDiscreteFunction(func, samplingFrequency);
        }

        public void SetInitialConditions()
        {
            DifferenceEquation.SetInitialCondition();
        }

        public double ProcessSample(double sample)
        {
           return DifferenceEquation.EvaluateNext(sample);
        }
    }
}