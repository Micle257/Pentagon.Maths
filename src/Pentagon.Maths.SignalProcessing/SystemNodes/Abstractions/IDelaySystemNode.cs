// -----------------------------------------------------------------------
//  <copyright file="IDelaySystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Abstractions
{
    public interface IDelaySystemNode : ISingleInputNode, IMemoryNode
    {
        int DelayLength { get; }
    }
}