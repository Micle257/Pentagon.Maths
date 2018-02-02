namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiInputFunctionNode : IMultiInputNode
    {
        readonly Func<double[], double> _function;

        public MultiInputFunctionNode(Func<double[], double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        double[] _values;

        int _index = -1;

        bool _isEvaluated;

        public double CurrentValue { get; private set; }
        /// <inheritdoc />
        public double GetValue(int index)
        {
            if (_values == null)
                _values = new double[InputNodes.Count];

            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            for (var i = 0; i < InputNodes.Count; i++)
            {
                _values[i] = InputNodes[i].GetValue(index);;
            }

            var value = _function(_values);

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return CurrentValue;
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (_values == null)
                _values = new double[inputValues.Length];

            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            for (var i = 0; i < inputValues.Length; i++)
            {
                _values[i] = inputValues[i];
            }

            var value = _function(_values);

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return CurrentValue;
        }

        /// <inheritdoc />
        public int InputCount { get; }

        /// <inheritdoc />
        public IList<INode> InputNodes { get; } = new List<INode>();

        /// <inheritdoc />
        public void AddInputNode(INode node)
        {
            if (!InputNodes.Contains(node))
                InputNodes.Add(node);
        }
    }
}