// -----------------------------------------------------------------------
//  <copyright file="EmptySystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Abstractions;

    public class EmptySystemNode : INode, ISingleInputNode
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues) => inputValues[0];
    }
}