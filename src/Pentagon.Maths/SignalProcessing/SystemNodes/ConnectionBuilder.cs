namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class ConnectionBuilder
    {
        IList<(IList<INode> tails, INode head)> _connections = new List<(IList<INode> tails, INode head)>();

        public ConnectionBuilder Connect(INode head, INode tail)
        {
            _connections.Add((new[] { tail }, head));
            return this;
        }

        public ConnectionBuilder Connect(INode head, params INode[] tails)
        {
            _connections.Add((tails, head));
            return this;
        }

        public IDictionary<INode, IList<INode>> Build()
        {
            var result = new ConcurrentDictionary<INode, IList<INode>>();

            foreach (var node in _connections)
            {
                result.TryAdd(node.head, node.tails);
            }

            return result;
        }
    }
}