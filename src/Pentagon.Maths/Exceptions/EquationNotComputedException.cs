// -----------------------------------------------------------------------
//  <copyright file="EquationNotComputedException.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Exceptions
{
    using System;

    public class EquationNotComputedException : Exception
    {
        public EquationNotComputedException() : base($"The equation is not computed.") { }
    }
}