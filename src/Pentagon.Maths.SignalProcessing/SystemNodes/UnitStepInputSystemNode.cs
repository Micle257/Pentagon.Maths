// -----------------------------------------------------------------------
//  <copyright file="UnitStepInputSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Abstractions;

    public class UnitStepInputSystemNode : IInputSystemNode
    {
        /// <inheritdoc />
        public int InputCount => 0;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues) => index >= 0 ? 1 : 0;

        /// <inheritdoc />
        public double GetValue(int index) => index >= 0 ? 1 : 0;
    }
}