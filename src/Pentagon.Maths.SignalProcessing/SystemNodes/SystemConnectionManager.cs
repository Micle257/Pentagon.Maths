namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public interface INodeSystem
    {
        void ConfigureConnections(IConnectionBuilder builder);
    } 

    public class SystemConnectionManager : SystemConnectionManager<object>
    {

    }

    public class SystemConnectionManager<T>
    {
        public T Instance { get; private set; }

        IDictionary<INode, IList<INode>> _connectionMap;

        IDictionary<INode, IList<INode>> ConnectionMap
        {
            get
            {
                if (_connectionMap != null)
                    return _connectionMap;

                if (Instance is INodeSystem ns)
                {
                    var b = new ConnectionBuilder();

                    ns.ConfigureConnections(b);

                    _connectionMap = b.Build();
                }

                return _connectionMap;
            }
        }

       INode _output;

        public void SetupConnection(Action<IConnectionBuilder, T> action)
        {
            var builder = new ConnectionBuilder();

            action(builder, Instance);

            _connectionMap = builder.Build();
        }

        IDictionary<INode, double> _values = new ConcurrentDictionary<INode, double>();

        IList<INode> _priority = new List<INode>();

        public void InitializeOutputNode(Func<T, INode> setup)
        {
            _output = setup(Instance);

            var g = new SystemNodeGrapher(_output, ConnectionMap);
            _priority = g.GetPriority();

            foreach (var node in _priority)
            {
                _values.Add(node, 0);
            }
        }

        public void InitializeOutputNode(INode output)
        {
            InitializeOutputNode(a => output);
        }

        public double GetValue(int index)
        {
            foreach (var n in _priority)
            {
                if (n is IInputSystemNode)
                    _values[n] = n.GetValue(index);
                else if (n is IMemoryNode mn)
                {
                    var last = mn.LastValue;
                    _values[n] = last;
                }
                else
                {
                    var inputs = _connectionMap[n];

                    var values = inputs.Select(a => _values[a]).ToArray();

                    _values[n] = n.GetValue(index, values);
                }
            }

            foreach (var n in _priority)
            {
                if (!(n is IMemoryNode))
                    continue;

                var inputs = _connectionMap[n];

                var values = inputs.Select(a => _values[a]).ToArray();

                n.GetValue(index, values);
            }

            return _values[_output];
        }

        public void SetupSystem(T system)
        {
            Instance = system;
        }
    }
}