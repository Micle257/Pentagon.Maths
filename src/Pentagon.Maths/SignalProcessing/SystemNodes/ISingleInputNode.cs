namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    public interface ISingleInputNode : INode
    {
        INode InputNode { get; }
        void SetInputNode(INode node);
    }
}