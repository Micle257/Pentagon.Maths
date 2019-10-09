// -----------------------------------------------------------------------
//  <copyright file="InputSampleSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Generic;
    using Abstractions;

    public class InputSampleSystemNode : IInputSystemNode
    {
        public IList<double> Values { get; } = new List<double>();

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues) => Values[index];

        public void Add(double value)
        {
            Values.Add(value);
        }

        /// <inheritdoc />
        public double GetValue(int index) => Values[index];
    }
}