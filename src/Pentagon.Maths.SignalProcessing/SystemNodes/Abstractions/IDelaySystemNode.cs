namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    public interface IDelaySystemNode : ISingleInputNode, IMemoryNode
    {
        int DelayLength { get; }
    }
}