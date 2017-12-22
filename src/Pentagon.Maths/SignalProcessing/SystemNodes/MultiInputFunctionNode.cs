namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiInputFunctionNode : IMultiInputNode
    {
        readonly Func<double[], double> _function;

        public MultiInputFunctionNode(Func<double[], double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            var values = InputNodes.Select(a => a.GetValue(index)).ToArray();

            return _function(values);
        }

        /// <inheritdoc />
        public ICollection<INode> InputNodes { get; } = new List<INode>();

        /// <inheritdoc />
        public void AddInputNode(INode node)
        {
            if (!InputNodes.Contains(node))
                InputNodes.Add(node);
        }
    }
}