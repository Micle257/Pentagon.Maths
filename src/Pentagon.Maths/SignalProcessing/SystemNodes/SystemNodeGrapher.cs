namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SystemNodeGrapher
    {
        readonly INode _node;
        readonly IDictionary<INode, IList<INode>> _connections;
        readonly List<IList<INode>> _nodes = new List<IList<INode>>();

        public SystemNodeGrapher(INode node, IDictionary<INode, IList<INode>> connections)
        {
            _node = node;
            _connections = connections;
        }

        public IList<INode> GetPriority()
        {
            var result = new List<INode>();

            _nodes.Add(new[] { _node });

            var conns = _connections[_node];

            Get(conns);

            foreach (var nodes in _nodes)
            {
                result.AddRange(nodes.Where(a => !result.Contains(a)));
            }

            result.Reverse();

            return result;
        }

        void Get(IList<INode> conns)
        {
            var toCompute = conns.Where(a => _connections.TryGetValue(a, out var c)).ToList();

            var toadd = new List<INode>();

            foreach (var node in toCompute)
            {
                if (!IsNodeRelated(node))
                {
                    toadd.Add(node);
                }
            }

            Get(toadd);

            _nodes.Add(toadd);
        }

        bool IsNodeRelated(INode node)
        {
            if (_connections.TryGetValue(node,out var inputs))
            {
                return inputs.Count != 0 && _nodes.SelectMany(a => a).Contains(node);
            }

            throw new ArgumentException();
        }
    }
}