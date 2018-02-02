namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    public class FilterSystemNode : ISingleInputNode
    {
        DifferenceEquation _eq;

        public INode InputNode { get; private set; }

        public int InputCount => 1;

        public FilterSystemNode(ISystemFunction system)
        {
            _eq = system as DifferenceEquation ?? new DifferenceEquation(system.Coefficients);
        }

        public FilterSystemNode(Expression<Func<RelativeSignal, RelativeSignal, double>> equationCallback)
        {
            _eq = DifferenceEquation.FromExpression(equationCallback);
        }

        bool _wasEvaluated;

        public double GetValue(int index, params double[] inputValues)
        {
            if (_wasEvaluated)
                return _eq.LastValue;

            _wasEvaluated = true;

            var next = inputValues[0];
            var value = _eq.EvaluateNext(next);

            _wasEvaluated = false;
            return value;
        }

        public double GetValue(int index)
        {
            if (_wasEvaluated)
                return _eq.LastValue;

            _wasEvaluated = true;

            var next = InputNode.GetValue(index);
            var value = _eq.EvaluateNext(next);

            _wasEvaluated = false;
            return value;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Filter system node" : $"{Name} (Filter)";
    }
}