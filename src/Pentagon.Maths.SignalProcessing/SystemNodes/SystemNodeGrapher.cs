// -----------------------------------------------------------------------
//  <copyright file="SystemNodeGrapher.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

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

        public SystemNodeGrapher(INode node, IDictionary<INode, IList<INode>> connections)
        {
            _node = node;
            _connections = connections;

            RelatedNodes = _connections.SelectMany(a => a.Value.Concat(new[] {a.Key})).Distinct().ToList();
            DelayNodes = RelatedNodes.Where(a => a is IDelaySystemNode).Select(a => (IDelaySystemNode) a).ToList();
            FilterNodes = RelatedNodes.Where(a => a is IFilterSystemNode).Select(a => (IFilterSystemNode)a).ToList();
            InputNodes = RelatedNodes.Where(a => a is IInputSystemNode).Select(a => (IInputSystemNode) a).ToList();
            FunctionalNodes = RelatedNodes.Except(InputNodes).Except(DelayNodes).ToList();
        }

        public IList<INode> RelatedNodes { get; }
        public IList<IDelaySystemNode> DelayNodes { get; }
        public IList<IFilterSystemNode> FilterNodes { get; }
        public IList<IInputSystemNode> InputNodes { get; }
        public IList<INode> FunctionalNodes { get; }

        public IList<INode> GetFunctionalPriority()
        {
            var overallPriority = new List<INode>();

            foreach (var inputNode in InputNodes)
            {
                var relatedNodes = _connections.Where(a => a.Value.Contains(inputNode)).Select(a => a.Key).ToList();
                var priority = new List<INode> {inputNode};
                var skippedNodes = new List<INode>();

                while (relatedNodes.Any())
                {
                    var inNodes = new List<List<INode>>();
                    foreach (var relatedNode in relatedNodes)
                    {
                        if (overallPriority.Contains(relatedNode))
                        {
                            skippedNodes.Add(relatedNode);
                            continue;
                        }

                        if (priority.Contains(relatedNode))
                            continue;

                        priority.Add(relatedNode);

                        inNodes.Add(_connections.Where(a => a.Value.Contains(relatedNode)).Select(a => a.Key).ToList());
                    }

                    relatedNodes.Clear();
                    relatedNodes.AddRange(inNodes.SelectMany(a => a));
                }

                if (overallPriority.Count == 0)
                    overallPriority.AddRange(priority);
                else
                {
                    var lessPriorityNode = overallPriority.FirstOrDefault(a => skippedNodes.Any(b => b == a));

                    var insertIndex = overallPriority.IndexOf(lessPriorityNode);

                    overallPriority.InsertRange(insertIndex, priority);
                }
            }

            //var result = new List<INode>();

            //var conns = _connections[_node];

            //Get(conns);

            //_nodes.Add(new[] {_node});

            //_nodes.Reverse();

            //foreach (var nodes in _nodes)
            //    result.AddRange(nodes.Where(a => !result.Contains(a)));

           var result = overallPriority.Where(a => !InputNodes.Contains(a)).ToList();

            return result;
        }

        void Get(IList<INode> conns)
        {
            var toCompute = conns.Where(a => _connections.TryGetValue(a, out var c)).ToList();

            var toadd = new List<INode>();

            foreach (var node in toCompute)
            {
                if (!IsNodeRelated(node))
                    toadd.Add(node);
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
                return inputs.Count != 0 && node == _node;

            throw new ArgumentException();
        }
    }
}