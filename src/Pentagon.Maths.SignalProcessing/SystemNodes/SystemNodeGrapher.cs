namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SystemNodeGrapher
    {
        readonly INode _node;
        readonly IDictionary<INode, IList<INode>> _connections;
        readonly List<IList<INode>> _nodes = new List<IList<INode>>();
        public IList<INode> RelatedNodes { get; }
        public IList<IMemoryNode> MemoryNodes { get; }
            public IList<IInputSystemNode> InputNodes { get; }
        public IList<INode> FunctionalNodes { get; }

        public SystemNodeGrapher(INode node, IDictionary<INode, IList<INode>> connections)
        {
            _node = node;
            _connections = connections;

            RelatedNodes = _connections.Values.SelectMany(a => a).Distinct().ToList();
            MemoryNodes = RelatedNodes.Where(a => a is IMemoryNode).Select(a => (IMemoryNode)a).ToList();
            InputNodes = RelatedNodes.Where(a => a is IInputSystemNode).Select(a => (IInputSystemNode)a).ToList();
            FunctionalNodes = RelatedNodes.Except(InputNodes).Except(MemoryNodes).ToList();
        }
    
        public IList<INode> GetFunctionalPriority()
        {
            var result = new List<INode>();
            
            var conns = _connections[_node];
            
            Get(conns);

            _nodes.Add(new [] {_node});

            _nodes.Reverse();

            foreach (var nodes in _nodes)
            {
                result.AddRange(nodes.Where(a => !result.Contains(a)));
            }

            result = result.Where(a => FunctionalNodes.Contains(a)).ToList();

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

            if (toadd.Count == 0)
                return;

            _nodes.Add(toadd);

            foreach (var node in toadd)
            {
                var cos = _connections[node];
                Get(cos);
            }
        }

        bool IsNodeRelated(INode node)
        {
            if (_connections.TryGetValue(node, out var inputs))
            {
                return inputs.Count != 0 && node == _node;
            }

            throw new ArgumentException();
        }
    }
}