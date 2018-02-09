namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public interface INodeSystem
    {
        INode Output { get; }
        void ConfigureConnections(IConnectionBuilder builder);
    }

    public class SystemConnectionManager : SystemConnectionManager<object>
    {
    }

    public class SystemConnectionManager<T>
    {
        public T Instance { get; private set; }

        IDictionary<INode, IList<INode>> _connectionMap;

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

            _grapher = new SystemNodeGrapher(_output, _connectionMap);
            _priority = _grapher.GetFunctionalPriority();

            foreach (var node in _grapher.RelatedNodes)
            {
                _values.Add(node, 0);
            }
        }
        List<SystemNodeWatcher> _watchers = new List<SystemNodeWatcher>();
        public void InitializeOutputNode(INode output)
        {
            InitializeOutputNode(a => output);
        }

        public void AddWatcher(SystemNodeWatcher watcher)
        {
            if (!_watchers.Contains(watcher))
                _watchers.Add(watcher);
        }

        double GetValueOutput(INode node, double value)
        {
            var con = _disableConstrains.FirstOrDefault(a => a.Node == node);

            if (con != null)
            {
                if (con.IsDisabled)
                    return 0;
                else if (con.Func((_values[node], null)))
                {
                    con.IsDisabled = true;
                    return 0;
                }
            }

            return value;
        }

        public double GetValue(int index)
        {
            // compute the values for input nodes
            foreach (var inputNode in _grapher.InputNodes)
            {
                _values[inputNode] = GetValueOutput(inputNode, inputNode.GetValue(index));
            }

            // compute the memory nodes
            foreach (var memoryNode in _grapher.MemoryNodes)
            {
                _values[memoryNode] = GetValueOutput(memoryNode, memoryNode.LastValue);
            }

            // get values from functional priority
            foreach (var n in _priority)
            {
                //if (n is IInputSystemNode)
                //    _values[n] = GetValueOutput(n, n.GetValue(index));
                //else if (n is IMemoryNode mn)
                //{
                //    _values[n] = GetValueOutput(mn, mn.LastValue);
                //}
                //else
                //{
                    var inputs = _connectionMap[n];

                    var values = inputs.Select(a => _values[a]).ToArray();

                    _values[n] = GetValueOutput(n, n.GetValue(index, values));
               // }
            }

            // compute next values for memory nodes
            foreach (var n in _grapher.MemoryNodes)
            {
                var inputs = _connectionMap[n];

                var values = inputs.Select(a => _values[a]).ToArray();

                n.GetValue(index, values);
            }

            if (_values.Any(a => a.Value > 100000000))
                Debugger.Break();
            
            foreach (var w in _watchers)
            {
                if (_values.ContainsKey(w.Node) && _connectionMap.ContainsKey(w.Node))
                w.AddValue(_values[w.Node], _connectionMap[w.Node].Select(a => _values[a]));
            }

            return _values[_output];
        }

        public void SetupSystem(T system)
        {
            Instance = system;

            if (Instance is INodeSystem ns)
            {
                var b = new ConnectionBuilder();
                ns.ConfigureConnections(b);
                _connectionMap = b.Build();

                InitializeOutputNode(ns.Output);
            }
        }

        public void AddDisableConstrain(SystemManagerDisableConstrain constrain)
        {
            if (!_disableConstrains.Contains(constrain))
                _disableConstrains.Add(constrain);
        }

        List<SystemManagerDisableConstrain> _disableConstrains = new List<SystemManagerDisableConstrain>();
        SystemNodeGrapher _grapher;
    }

    public class SystemManagerDisableConstrain
    {
        public INode Node { get; }
        public Func<(double Value, double[] Input), bool> Func { get; }
        public bool IsDisabled { get; set; }

        public SystemManagerDisableConstrain(INode node, Func<(double Value, double[] Input), bool> func)
        {
            Node = node;
            Func = func;
        }
    }
}