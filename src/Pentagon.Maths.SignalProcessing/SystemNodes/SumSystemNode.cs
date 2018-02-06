namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class SumSystemNode : IMultiInputNode
    {
        bool _isEvaluated;

        int _index = -1;

        public double CurrentValue { get; private set; }
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var sum = 0d;
            foreach (var node in InputNodes)
            {
                sum += node.GetValue(index);
            }

            _index = index;
            _isEvaluated = true;
            CurrentValue = sum;

            return CurrentValue;
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var sum = 0d;
            foreach (var node in inputValues)
            {
                sum += node;
            }

            _index = index;
            _isEvaluated = true;
            CurrentValue = sum;

            return CurrentValue;
        }

        /// <inheritdoc />
        public int InputCount { get; }

        /// <inheritdoc />
        public IList<INode> InputNodes { get; } = new Collection<INode>();

        /// <inheritdoc />
        public void AddInputNode(INode node)
        {
            if (InputNodes.Contains(node))
                return;
            InputNodes.Add(node);
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Sum system node" : $"{Name} (Sum)";
    }
}