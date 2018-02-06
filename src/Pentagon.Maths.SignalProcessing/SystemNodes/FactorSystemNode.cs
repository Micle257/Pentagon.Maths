namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Pentagon.Extensions;

    public class FactorSystemNode : INode, ISingleInputNode
    {
        public double Factor { get; }
        public INode InputNode { get; private set; }

        bool _isEvaluated;

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        public FactorSystemNode(double factor)
        {
            Factor = factor;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        int _index = -1;

        public double CurrentValue { get; private set; }

        public double GetValue(int index)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var inputValue = InputNode.GetValue(index);
            var value = inputValue * Factor;

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return value;
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var inputValue = inputValues[0];
            var value = inputValue * Factor;

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return value;
        }

        /// <inheritdoc />
        public int InputCount => 1;

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Factor: {Factor.RoundSignificantFigures(4)}" : $"{Name} (Factor): {Factor.RoundSignificantFigures(4)}";
    }
}