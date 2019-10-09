// -----------------------------------------------------------------------
//  <copyright file="ConnectionBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstractions;

    public class ConnectionBuilder : IConnectionBuilder
    {
        readonly IDictionary<INode, IList<INode>> _connections = new ConcurrentDictionary<INode, IList<INode>>();

        public IConnectionBuilder Connect(INode head, INode tail)
        {
            if (_connections.TryGetValue(head, out var inputs))
                inputs.Add(tail);
            else
                _connections.Add(head, new List<INode> {tail});

            return this;
        }

        public IConnectionBuilder Connect(INode head, params INode[] tails)
        {
            if (_connections.TryGetValue(head, out var inputs))
            {
                foreach (var tail in tails)
                    inputs.Add(tail);
            }
            else
                _connections.Add(head, tails);

            return this;
        }

        public IDictionary<INode, IList<INode>> Build() => _connections;
    }
}