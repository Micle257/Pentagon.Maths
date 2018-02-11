namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    public interface IFilterSystemNode : ISingleInputNode, IMemoryNode
    {
        ISystemFunction System { get; }
    }
}