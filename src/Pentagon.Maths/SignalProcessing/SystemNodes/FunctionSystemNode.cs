namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Diagnostics;

    public class FunctionSystemNode : ISingleInputNode
    {
        readonly Func<double, double> _function;

        public FunctionSystemNode(Func<double,double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            var value = InputNode.GetValue(index);
            
            var output = _function(value);

            return output;
        }

        /// <inheritdoc />
        public INode InputNode { get; private set; }

        /// <inheritdoc />
        public void SetInputNode(INode node)
        {
            InputNode = node;
        }
    }
}