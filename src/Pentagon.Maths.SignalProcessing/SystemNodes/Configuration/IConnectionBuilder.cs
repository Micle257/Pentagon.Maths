// -----------------------------------------------------------------------
//  <copyright file="IConnectionBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System.Collections.Generic;
    using Abstractions;

    public interface IConnectionBuilder
    {
        IConnectionBuilder Connect(INode head, INode tail);
        IConnectionBuilder Connect(INode head, params INode[] tails);
        IDictionary<INode, IList<INode>> Build();
    }
}