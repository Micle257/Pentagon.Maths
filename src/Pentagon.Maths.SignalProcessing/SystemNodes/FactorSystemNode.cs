namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using Pentagon.Extensions;

    public class FactorSystemNode : INode, ISingleInputNode
    {
        public double Factor { get; }
        
        public FactorSystemNode(double factor)
        {
            Factor = factor;
        }

        /// <inheritdoc />
        public string Name { get; set; }
        
        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var inputValue = inputValues[0];
            var value = inputValue * Factor;
            
            return value;
        }
        
        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Factor: {Factor.RoundSignificantFigures(4)}" : $"{Name} (Factor): {Factor.RoundSignificantFigures(4)}";
    }
}