namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class SumSystemNode : IMultiInputNode
    {
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
    }
}