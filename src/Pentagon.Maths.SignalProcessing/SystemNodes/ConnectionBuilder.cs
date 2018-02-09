namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class SystemNodeWatcher
    {
        public IList<double> OutputValues { get; } = new List<double>();
        public IList<double[]> InputValues { get; } = new List<double[]>();
        public INode Node { get; }

        public SystemNodeWatcher(INode node)
        {
            Node = node;
        }

        public void AddValue(double output, IEnumerable<double> input)
        {
            OutputValues.Add(output);
            InputValues.Add(input.ToArray());
        }
    }

    public static class ConnectionBuilderExtensions
    {
        public static IConnectionBuilder Connect(this IConnectionBuilder builder, INode node, INodeSystem system)
        {
            builder.Connect(node, system.Output);

            return builder;
        }
    }

    public interface IConnectionBuilder
    {
        IConnectionBuilder Connect(INode head, INode tail);
        IConnectionBuilder Connect(INode head, params INode[] tails);
        IDictionary<INode, IList<INode>> Build();
    }

    public class ConnectionBuilder : IConnectionBuilder
    {
        IDictionary<INode, IList<INode>> _connections = new ConcurrentDictionary<INode, IList<INode>>();

        public IConnectionBuilder Connect(INode head, INode tail)
        {
            if (_connections.TryGetValue(head, out var inputs))
            {
                inputs.Add(tail);
            }
            else
                _connections.Add(head, (new List<INode> { tail }));

            return this;
        }

        public IConnectionBuilder Connect(INode head, params INode[] tails)
        {
            if (_connections.TryGetValue(head, out var inputs))
            {
                foreach (var tail in tails)
                {
                    inputs.Add(tail);
                }
            }
            else
                _connections.Add(head, tails);

            return this;
        }

        public IDictionary<INode, IList<INode>> Build()
        {
            //var result = new ConcurrentDictionary<INode, IList<INode>>();

            //foreach (var node in _connections)
            //{
            //    result.TryAdd(node.head, node.tails);
            //}

            //return result;
            return _connections;
        }
    }
}