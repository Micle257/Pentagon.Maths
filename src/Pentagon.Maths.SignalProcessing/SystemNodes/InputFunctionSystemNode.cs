// -----------------------------------------------------------------------
//  <copyright file="InputFunctionSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Abstractions;
    using Functions;

    public class InputFunctionSystemNode : IInputSystemNode
    {
        readonly IDiscreteFunction _function;

        public InputFunctionSystemNode(IDiscreteFunction function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public int InputCount => 0;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var value = _function.EvaluateSample(index);
            return value;
        }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            var value = _function.EvaluateSample(index);
            return value;
        }
    }
}