// -----------------------------------------------------------------------
//  <copyright file="ConvolutionSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Generic;
    using Abstractions;

    public class ConvolutionSystemNode : ISingleInputNode
    {
        public ConvolutionSystemNode(double[] knownSignal)
        {
            KnownSignal = knownSignal;
        }

        public double[] KnownSignal { get; }

        public List<double> InputValues { get; } = new List<double>();

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var input = inputValues[0];
            InputValues.Add(input);

            var c = 0d;
            for (var i = 0; i < index && i < KnownSignal.Length; i++)
            {
                var a = InputValues[i];
                var b = KnownSignal[index - i];
                c += a * b;
            }

            return c;
        }
    }
}