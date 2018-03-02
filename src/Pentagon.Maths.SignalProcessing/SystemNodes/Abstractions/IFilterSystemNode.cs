// -----------------------------------------------------------------------
//  <copyright file="IFilterSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Abstractions
{
    public interface IFilterSystemNode : ISingleInputNode, IMemoryNode
    {
        ISystemFunction System { get; }
    }
}