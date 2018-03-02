// -----------------------------------------------------------------------
//  <copyright file="FactorSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Abstractions;
    using Pentagon.Extensions;

    public class FactorSystemNode : INode, ISingleInputNode
    {
        public FactorSystemNode(double factor)
        {
            Factor = factor;
        }

        public double Factor { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var inputValue = inputValues[0];
            var value = inputValue * Factor;

            return value;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Factor: {Factor.RoundSignificantFigures(4)}" : $"{Name} (Factor): {Factor.RoundSignificantFigures(4)}";
    }
}