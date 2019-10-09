// -----------------------------------------------------------------------
//  <copyright file="SystemConnectionManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    public class SystemConnectionManager : SystemConnectionManager<object> { }

    public class SystemConnectionManager<T>
    {
        readonly IDictionary<INode, double> _values = new ConcurrentDictionary<INode, double>();
        readonly List<SystemNodeWatcher> _watchers = new List<SystemNodeWatcher>();
        IDictionary<INode, IList<INode>> _connectionMap;

        INode _output;

        IList<INode> _priority = new List<INode>();

        SystemNodeGrapher _grapher;
        public SystemManagerConstrainCollection Constrains { get; } = new SystemManagerConstrainCollection();
        public T Instance { get; private set; }

        public void SetupConnection(Action<IConnectionBuilder, T> action)
        {
            var builder = new ConnectionBuilder();

            action(builder, Instance);

            _connectionMap = builder.Build();
        }

        public void InitializeOutputNode(Func<T, INode> setup)
        {
            _output = setup(Instance);

            _grapher = new SystemNodeGrapher(_output, _connectionMap);
            _priority = _grapher.GetFunctionalPriority();

            foreach (var node in _grapher.RelatedNodes)
                _values.Add(node, 0);
        }

        public void InitializeOutputNode(INode output)
        {
            InitializeOutputNode(a => output);
        }

        public void AddWatcher(SystemNodeWatcher watcher)
        {
            if (!_watchers.Contains(watcher))
                _watchers.Add(watcher);
        }

        public double GetValue(int index)
        {
            GetValueCore(index);

            var force = false;
            foreach (var d in _values)
                force = force || CheckConstrains(d.Key, d.Value, null);

            // if (force)
            //    GetValueCore(index);

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

        void GetValueCore(int index)
        {
            // compute the values for input nodes
            foreach (var inputNode in _grapher.InputNodes)
                _values[inputNode] = GetValueOutput(inputNode, inputNode.GetValue(index));

            // compute the values for delay outputs
            foreach (var inputNode in _grapher.DelayOutputNodes)
            {
                var delay = inputNode.Delay;

                _values[inputNode] = _values[delay];
            }

            // get values from functional priority
            foreach (var n in _priority)
            {
                var inputs = _connectionMap[n];

                var fixedInputs = inputs.ToList();

                foreach (var input in inputs)
                {
                    var inDelay = _grapher.DelayNodes.FirstOrDefault(a => a == input);
                    if (inDelay != null)
                    {
                        var by = _grapher.DelayOutputNodes.FirstOrDefault(a => a.Delay == inDelay);
                        if (by != null)
                        {
                            fixedInputs.Remove(input);
                            fixedInputs.Add(by);
                        }
                    }
                }

                var values = fixedInputs.Select(a => _values[a]).ToArray();

                var value = n.GetValue(index, values);

                _values[n] = value;
            }
        }

        bool CheckConstrains(INode node, double output, double[] inputs)
        {
            var con = Constrains.Items.FirstOrDefault(a => a.Node == node);

            return con?.Run(_values[node], inputs) ?? false;
        }

        double GetValueOutput(INode node, double value)
        {
            var con = Constrains.Items.FirstOrDefault(a => a.Node == node);

            con?.Run(_values[node], null);

            return value;
        }
    }
}