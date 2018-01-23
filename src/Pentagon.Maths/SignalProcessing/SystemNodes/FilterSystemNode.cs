namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    public class SystemConnectionManager
    {
        IList<INode> _nodes = new List<INode>();

        public void Add(INode node)
        {
            if (!_nodes.Contains(node))
                _nodes.Add(node);
        }

        public void AddRange(params INode[] nodes)
        {
            foreach (var node in nodes)
            {
                Add(node);
            }
        }

        public void SetupConnection(Action<ConnectionBuilder> action)
        {
            var builder = new ConnectionBuilder();

            action(builder);

            builder.Config(_nodes);
        }
    }

    public class ConnectionBuilder
    {
        IList<(IList<INode> tails, INode head)> _connections = new List<(IList<INode> tails, INode head)> ();

        public ConnectionBuilder Connect( INode head, INode tail)
        {
            _connections.Add((new []{ tail}, head));
            return this;
        }

        public ConnectionBuilder Connect(INode head, params INode[] tails)
        {
            _connections.Add((tails, head));
            return this;
        }

        public void Config(IList<INode> nodes)
        {
            foreach (var node in _connections)
            {
                var n = node.head;

                //if (!nodes.Contains(n))
                //    throw new ArgumentException("The node is not in nodes collection.");

                switch (n)
                {
                    case ISingleInputNode sin:
                        sin.SetInputNode(node.tails.FirstOrDefault());
                        break;

                    case IMultiInputNode min:
                       foreach (var tn in node.tails)
                       {
                           min.AddInputNode(tn);
                       }
                       break;
                }
            }
        }
    }

    public class FilterSystemNode : IMemoryNode, ISingleInputNode
    {
        DifferenceEquation _eq;
        public IList<double> Values { get; } = new List<double>();

        public INode InputNode { get; private set; }

        public FilterSystemNode(Expression<DifferenceEquationCallback> equationCallback)
        {
            _eq = new DifferenceEquation(equationCallback);
        }

        public FilterSystemNode(DifferenceEquation equation)
        {
            _eq = equation;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        public double GetValue(int index)
        {
            if (index < Values.Count)
                return Values[index];

            if (index > Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (Values.Count > 0)
                Values.Add(Values[Values.Count - 1]);
            else
                Values.Add(0);

            var preValue = InputNode.GetValue(index);
            var value = _eq.EvaluateNext(preValue);
            
            Values[Values.Count - 1] = value;

            return Values[index];
        }

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Filter system node" : $"{Name} (Filter)";
    }
}