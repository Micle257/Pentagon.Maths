namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using Pentagon.Extensions;

    public class FactorSystemNode : INode, ISingleInputNode
    {
        public double Factor { get; }
        public INode InputNode { get; private set; }

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

        public double GetValue(int index)
        {
            var inputValue = InputNode.GetValue(index);

            return inputValue * Factor;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Factor: {Factor.RoundSignificantFigures(4)}" : $"{Name} (Factor): {Factor.RoundSignificantFigures(4)}";
    }
}