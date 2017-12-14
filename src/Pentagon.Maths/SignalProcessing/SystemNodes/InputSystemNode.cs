namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using Functions;

    public abstract class InputSystemNode : INode
    {
        public abstract double GetValue(int index);
    }

    public class InputFunctionSystemNode : InputSystemNode
    {
        readonly IDiscreteFunction _function;

        public InputFunctionSystemNode(IDiscreteFunction function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public override double GetValue(int index)
        {
            var value = _function.EvaluateSample(index);
            return value;
        }
    }
}