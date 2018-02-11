namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    public class FilterSystemNode : IFilterSystemNode, ISingleInputNode, IMemoryNode
    {
        DifferenceEquation _eq;

        public ISystemFunction System { get; }
        
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
        
        /// <inheritdoc />
        public string Name { get; set; }
        
        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Filter system node" : $"{Name} (Filter)";

        /// <inheritdoc />
        public double LastValue => _eq.LastValue;
    }
}