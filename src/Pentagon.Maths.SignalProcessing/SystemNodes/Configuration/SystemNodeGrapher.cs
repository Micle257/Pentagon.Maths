// -----------------------------------------------------------------------
//  <copyright file="SystemNodeGrapher.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Collections;

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
            FilterNodes = RelatedNodes.Where(a => a is IFilterSystemNode).Select(a => (IFilterSystemNode) a).ToList();
            InputNodes = RelatedNodes.Where(a => a is IInputSystemNode).Select(a => (IInputSystemNode) a).ToList();
            FunctionalNodes = RelatedNodes.Except(InputNodes).Except(DelayNodes).ToList();
        }

        public IList<INode> RelatedNodes { get; }
        public IList<IDelaySystemNode> DelayNodes { get; }
        public IList<IFilterSystemNode> FilterNodes { get; }
        public IList<IInputSystemNode> InputNodes { get; }
        public IList<INode> FunctionalNodes { get; }
        public IList<DelayOutputSystemNode> DelayOutputNodes { get; } = new List<DelayOutputSystemNode>();

        public IList<INode> GetFunctionalPriority()
        {
            var overallPriority = new List<INode>();

            foreach (var inputNode in InputNodes)
            {
                var relatedNodes = _connections.Where(a => a.Value.Contains(inputNode)).Select(a => a.Key).ToList();

                var tree = new HierarchyList<INode>(inputNode);
                foreach (var relatedNode in relatedNodes)
                    tree.Root.AddChildren(relatedNode);

                var currentItem = new List<HierarchyListItem<INode>> {tree.Root};

                var priority = new List<INode> {inputNode};
                var skippedNodes = new List<INode>();

                while (relatedNodes.Any())
                {
                    var inNodes = new Dictionary<INode, List<INode>>();
                    foreach (var relatedNode in relatedNodes)
                    {
                        if (overallPriority.Contains(relatedNode))
                        {
                            skippedNodes.Add(relatedNode);
                            continue;
                        }

                        if (priority.Contains(relatedNode))
                        {
                            var relatedHierItem = currentItem.SelectMany(a => a.Children).FirstOrDefault(b => b.Value == relatedNode);

                            // check if there is the connection to related node from inputNode  
                            var connectedNode = _connections.FirstOrDefault(a => a.Key == relatedNode).Value
                                                            .FirstOrDefault(a => a == relatedHierItem.Parent);

                            if (connectedNode == null) //connection is not processed
                            {
                                var nodeCursor = relatedHierItem;
                                var chain = new KeyValuePair<INode, IList<INode>>(relatedNode, new List<INode>());
                                while (nodeCursor != null) // null would be if we get to the root item
                                {
                                    if (nodeCursor.Parent.IsRoot)
                                        break;

                                    var parent = nodeCursor.Parent.Value;

                                    if (parent is IDelaySystemNode dsn)
                                    {
                                        var del = DelayOutputNodes.FirstOrDefault(a => a.Delay == dsn);

                                        if (del == null)
                                        {
                                            var outputDelay = new DelayOutputSystemNode(dsn);
                                            DelayOutputNodes.Add(outputDelay);
                                            chain.Value.Add(outputDelay);
                                        }

                                        break;
                                    }

                                    if (parent == relatedNode)
                                        throw new OverflowException(message: "The node system computation loop.");

                                    nodeCursor = nodeCursor.Parent;
                                    chain.Value.Add(nodeCursor.Value);
                                }

                                // remove all nodes in chain from priority
                                foreach (var node in chain.Value)
                                    priority.Remove(node);

                                var insertIndex = priority.IndexOf(chain.Key);

                                priority.InsertRange(insertIndex, chain.Value.Reverse());
                            }

                            continue;
                        }

                        priority.Add(relatedNode);

                        inNodes.Add(relatedNode, _connections.Where(a => a.Value.Contains(relatedNode)).Select(a => a.Key).ToList());
                    }

                    var parents = new List<HierarchyListItem<INode>>();
                    for (var i = 0; i < currentItem.Count; i++)
                    {
                        for (var j = 0; j < relatedNodes.Count; j++)
                        {
                            var parent = currentItem[i].Children.FirstOrDefault(a => a.Value == relatedNodes[j]);

                            if (parent == null)
                                continue;

                            parents.Add(parent);

                            if (!inNodes.TryGetValue(relatedNodes[j], out var children))
                                continue;

                            foreach (var child in children)
                                parent.AddChildren(child);
                        }
                    }

                    relatedNodes.Clear();
                    relatedNodes.AddRange(inNodes.Values.SelectMany(a => a));

                    currentItem.Clear();
                    currentItem.AddRange(parents);
                }

                if (overallPriority.Count == 0)
                    overallPriority.AddRange(priority);
                else
                {
                    var lessPriorityNode = overallPriority.FirstOrDefault(a => skippedNodes.Any(b => b == a));

                    if (lessPriorityNode == null)
                        continue;

                    var insertIndex = overallPriority.IndexOf(lessPriorityNode);

                    overallPriority.InsertRange(insertIndex, priority);
                }

                //overallPriority.Where(a => a is DelayOutputSystemNode).Cast<DelayOutputSystemNode>().GroupBy(a => a.Delay).Select(a => a.First());
            }

            var result = overallPriority.Where(a => !InputNodes.Contains(a) && !DelayOutputNodes.Contains(a)).ToList();

            return result;
        }
    }
}