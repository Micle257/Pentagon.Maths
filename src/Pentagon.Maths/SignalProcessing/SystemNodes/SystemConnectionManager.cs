namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class SystemConnectionManager
    {
        IDictionary<INode, IList<INode>> _connectionMap;
        
        public void SetupConnection(Action<ConnectionBuilder> action)
        {
            var builder = new ConnectionBuilder();

            action(builder);

            _connectionMap = builder.Build();
        }

        IDictionary<INode, double> _values = new  ConcurrentDictionary<INode, double>();

        IList<INode> _priority = new List<INode>();

        public void InitializeOutputNode(INode output)
        {
            var g = new SystemNodeGrapher(output, _connectionMap);
            _priority = g.GetPriority();

            foreach (var node in _priority)
            {
                _values.Add(node, 0);
            }
        }
        
        public double GetValue(int index)
        {
            var result = 0d;

            foreach (var n in _priority)
            {
                if (n is IInputNode)
                    _values[n] = n.GetValue(index);

                if (_connectionMap.TryGetValue(n, out var inputs))
                {
                    var values = inputs.Select(a =>
                                               {
                                                   if (_values.TryGetValue(a, out var vals))
                                                       return vals;

                                                   throw new ArgumentException();
                                               });

                    _values[n] = n.GetValue(index, values.ToArray());
                }
            }

            return result;
        }

        void Config()
        {
            foreach (var node in _connectionMap)
            {
                var n = node.Key;

                switch (n)
                {
                    case ISingleInputNode sin:
                        sin.SetInputNode(node.Value.FirstOrDefault());
                        break;

                    case IMultiInputNode min:
                        foreach (var tn in node.Value)
                        {
                            min.AddInputNode(tn);
                        }
                        break;
                }
            }
        }
    }
}