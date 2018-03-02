// -----------------------------------------------------------------------
//  <copyright file="ILinearSystem.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using Functions;
    using Quantities;

    public interface ILinearSystem
    {
        IDiscreteFunction GetImpulseResponse(Frequency samplingFrequency);
    }
}