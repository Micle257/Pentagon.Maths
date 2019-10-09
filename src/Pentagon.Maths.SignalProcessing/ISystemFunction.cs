// -----------------------------------------------------------------------
//  <copyright file="ISystemFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    public interface ISystemFunction
    {
        SystemTuple Coefficients { get; }
    }
}