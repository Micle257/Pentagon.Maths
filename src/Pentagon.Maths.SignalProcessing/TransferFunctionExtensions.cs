// -----------------------------------------------------------------------
//  <copyright file="TransferFunctionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    public static class TransferFunctionExtensions
    {
        public static DifferenceEquation ToDifferenceEquation(this TransferFunction function) => DifferenceEquation.FromTransferFunction(function);
    }
}