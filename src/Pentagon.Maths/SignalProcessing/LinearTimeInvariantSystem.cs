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
        public ISystemDefinition Definition { get; }

        public LinearTimeInvariantSystem(ISystemDefinition definition)
        {
            Definition = definition;
        }
        
        public DiscreteFunction GetImpulseResponse(Frequency samplingFrequency)
        {
            var imp = InfiniteDiscreteFunction.ImpulseFunction(samplingFrequency);
            Func<int, double> func = i =>
                                     {
                                         if (i < 0)
                                             return 0;
                                         var d = 0d;
                                         for (var j = 0; j < i + 1; j++)
                                           d=  Definition.EvaluateNext(imp.EvaluateSample(j));
                                         return d;
                                     };
            return new DiscreteFunction(func, samplingFrequency);
        }

        public double ProcessSample(double sample)
        {
           return Definition.EvaluateNext(sample);
        }
    }
}