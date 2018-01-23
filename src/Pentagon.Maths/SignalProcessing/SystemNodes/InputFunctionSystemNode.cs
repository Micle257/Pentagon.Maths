namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using Functions;

    public class InputFunctionSystemNode : IInputSystemNode
    {
        readonly IDiscreteFunction _function;

        public InputFunctionSystemNode(IDiscreteFunction function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            var value = _function.EvaluateSample(index);
            return value;
        }
    }
}