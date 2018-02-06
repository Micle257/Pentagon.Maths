namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;

    public interface IMemoryNode : INode
    {
        double LastValue { get; }
    }
}