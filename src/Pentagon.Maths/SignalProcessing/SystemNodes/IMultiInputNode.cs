namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;

    public interface IMultiInputNode : INode
    {
        IList<INode> InputNodes { get; }
        void AddInputNode(INode node);
    }
}