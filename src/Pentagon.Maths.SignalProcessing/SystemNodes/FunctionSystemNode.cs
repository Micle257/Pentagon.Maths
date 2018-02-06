namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Diagnostics;

    public class FunctionSystemNode : ISingleInputNode
    {
        readonly Func<double, double> _function;

        bool _isEvaluated;

        public FunctionSystemNode(Func<double,double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        int _index = -1;

        public double CurrentValue { get; private set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var value = InputNode.GetValue(index);
            var output = _function(value);

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return output;
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (_isEvaluated && _index == index)
                return CurrentValue;

            _isEvaluated = false;

            var value = inputValues[0];
            var output = _function(value);

            _index = index;
            _isEvaluated = true;
            CurrentValue = value;

            return output;
        }

        /// <inheritdoc />
        public int InputCount => 1;

        /// <inheritdoc />
        public INode InputNode { get; private set; }

        /// <inheritdoc />
        public void SetInputNode(INode node)
        {
            InputNode = node;
        }
    }
}