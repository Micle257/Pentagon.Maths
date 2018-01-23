namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class SumSystemNode : IMultiInputNode
    {
        /// <inheritdoc />
        public string Name { get; set; }

        public double GetValue(int index)
        {
            var sum = 0d;
            foreach (var node in InputNodes)
            {
                sum += node.GetValue(index);
            }
            
            return sum;
        }
        
        public ICollection<INode> InputNodes { get; } = new Collection<INode>();

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