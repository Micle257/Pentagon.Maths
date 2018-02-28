// -----------------------------------------------------------------------
//  <copyright file="SystemConnectionManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

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
        void ConfigureConstrains(SystemManagerConstrainCollection contrains);
        void Initialize();
    }

    public class SystemManagerConstrainCollection
    {
       public List<ISystemManagerConstrain> Items { get; } = new List<ISystemManagerConstrain>();

        public void Add<TModel>(TModel model, Func<TModel, INode> noteSelector, Func<(double OutputValue, double[] InputValues), bool> condition, Action<TModel> action)
            where TModel : INodeSystem
        {
            Items.Add(new SystemManagerConstrain<TModel>(model, noteSelector, condition,action));
        }
    }

    public interface ISystemManagerConstrain {
        INode Node { get; }
        void Run(double outputValue, IEnumerable<double> inputValues);
    }

    public class SystemManagerConstrain<TModel> : ISystemManagerConstrain
        where TModel : INodeSystem
    {
        readonly TModel _model;
        readonly Func<TModel, INode> _noteSelector;
        readonly Func<(double OutputValue, double[] InputValues), bool> _condition;
        readonly Action<TModel> _action;

        public SystemManagerConstrain(TModel model, Func<TModel, INode> noteSelector, Func<(double OutputValue, double[] InputValues), bool> condition, Action<TModel> action)
        {
            _model = model;
            _condition = condition;
            _action = action;

            Node = noteSelector(_model);
        }

        /// <inheritdoc />
        public INode Node { get; }

        public void Run(double outputValue, IEnumerable<double> inputValues)
        {
            var result = _condition((outputValue, inputValues?.ToArray()));

            if (result)
                _action(_model); // TODO make togglable
        }
    }

    public class SystemConnectionManager : SystemConnectionManager<object> { }

    public class SystemConnectionManager<T>
    {
        IDictionary<INode, IList<INode>> _connectionMap;

        INode _output;

        readonly IDictionary<INode, double> _values = new ConcurrentDictionary<INode, double>();

        IList<INode> _priority = new List<INode>();
        readonly List<SystemNodeWatcher> _watchers = new List<SystemNodeWatcher>();
        
        SystemNodeGrapher _grapher;
        public T Instance { get; private set; }
        public SystemManagerConstrainCollection Constrains { get; } = new SystemManagerConstrainCollection();

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

                var value = GetValueOutput(n, n.GetValue(index, values));

                _values[n] = value;
            }

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

        double GetValueOutput(INode node, double value)
        {
            var con = Constrains.Items.FirstOrDefault(a => a.Node == node);

            con?.Run(_values[node], null);

            return value;
        }
    }

    public class SystemManagerDisableConstrain
    {
        public SystemManagerDisableConstrain(INode node, Func<(double Value, double[] Input), bool> func)
        {
            Node = node;
            Func = func;
        }

        public INode Node { get; }
        public Func<(double Value, double[] Input), bool> Func { get; }
        public bool IsDisabled { get; set; }
    }
}