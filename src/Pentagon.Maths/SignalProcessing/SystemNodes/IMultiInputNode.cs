namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;

    public interface IMultiInputNode : INode
    {
        ICollection<INode> InputNodes { get; }
        void AddInputNode(INode node);
    }
}