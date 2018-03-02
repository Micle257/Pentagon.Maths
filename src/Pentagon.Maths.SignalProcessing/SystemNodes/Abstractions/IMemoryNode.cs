// -----------------------------------------------------------------------
//  <copyright file="IMemoryNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Abstractions
{
    public interface IMemoryNode : INode
    {
        double LastValue { get; }
    }
}