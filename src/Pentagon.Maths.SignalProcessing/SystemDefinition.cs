// -----------------------------------------------------------------------
//  <copyright file="SystemDefinition.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    public class SystemDefinition : ISystemFunction
    {
        public SystemDefinition(SystemTuple coefficients)
        {
            Coefficients = coefficients;
        }

        public SystemDefinition(double[] numerator, double[] denumeretor) : this(new SystemTuple(numerator, denumeretor)) { }

        /// <inheritdoc />
        public SystemTuple Coefficients { get; }
    }
}