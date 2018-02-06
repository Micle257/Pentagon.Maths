namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public interface IConnectionBuilder {
        IConnectionBuilder Connect(INode head, INode tail);
        IConnectionBuilder Connect(INode head, params INode[] tails);
        IDictionary<INode, IList<INode>> Build();
    }

    public class ConnectionBuilder : IConnectionBuilder
    {
        IList<(IList<INode> tails, INode head)> _connections = new List<(IList<INode> tails, INode head)>();

        public IConnectionBuilder Connect(INode head, INode tail)
        {
            _connections.Add((new[] { tail }, head));
            return this;
        }

        public IConnectionBuilder Connect(INode head, params INode[] tails)
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