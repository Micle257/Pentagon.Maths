namespace Pentagon.Maths.SignalProcessing.SystemNodes {
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

        public double GetValue(int index)
        {

            var inputValue = InputNode.GetValue(index);

            return inputValue * Factor;
        }
    }
}