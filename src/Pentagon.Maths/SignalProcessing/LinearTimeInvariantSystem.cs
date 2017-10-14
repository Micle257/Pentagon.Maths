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
        public LinearTimeInvariantSystem(TransferFunction function)
        {
            Function = function;
        }

        public TransferFunction Function { get; }

        public DiscreteFunction GetImpulseResponse()
        {
            var imp = DiscreteFunction.ImpulseFunction();
            Func<int, double> func = i =>
                                     {
                                         var f = new IirDigitalFilter(Function);
                                         if (i < 0)
                                             return 0;
                                         var d = 0d;
                                         for (var j = 0; j < i + 1; j++)
                                             d = f.ProcessSample(imp.EvaluateSample(j));
                                         f.SetInitialCondition();
                                         return d;
                                     };
            return new DiscreteFunction(func, Frequency.Infinity);
        }
    }
}