// -----------------------------------------------------------------------
//  <copyright file="DelayOutputSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Abstractions;

    public class DelayOutputSystemNode : ISingleInputNode
    {
        public DelayOutputSystemNode(IDelaySystemNode delay)
        {
            Delay = delay;
        }

        public IDelaySystemNode Delay { get; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues) => Delay.LastValue;
    }
}