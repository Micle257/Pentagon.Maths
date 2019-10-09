// -----------------------------------------------------------------------
//  <copyright file="InputFunctionSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using Abstractions;

    public class InputFunctionSystemNode : IInputSystemNode
    {
        readonly Func<int, double> _function;

        public InputFunctionSystemNode(Func<int, double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var value = _function(index);
            return value;
        }
    }
}