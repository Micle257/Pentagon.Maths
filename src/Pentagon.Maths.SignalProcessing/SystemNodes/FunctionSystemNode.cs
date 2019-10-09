// -----------------------------------------------------------------------
//  <copyright file="FunctionSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using Abstractions;

    public class FunctionSystemNode : ISingleInputNode
    {
        readonly Func<double, double> _function;

        public FunctionSystemNode(Func<double, double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var value = inputValues[0];
            var output = _function(value);

            return output;
        }
    }
}