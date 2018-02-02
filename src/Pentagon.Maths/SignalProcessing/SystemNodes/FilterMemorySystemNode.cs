namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class FilterMemorySystemNode : IMemoryNode, ISingleInputNode
    {
        DifferenceEquation _eq;
        public IList<double> Values { get; } = new List<double>();

        public INode InputNode { get; private set; }

        public FilterMemorySystemNode(ISystemFunction system)
        {
            _eq = system as DifferenceEquation ?? new DifferenceEquation(system.Coefficients);
        }

        public FilterMemorySystemNode(Expression<Func<RelativeSignal, RelativeSignal, double>> equationCallback)
        {
            _eq = DifferenceEquation.FromExpression(equationCallback);
        }

        /// <inheritdoc />
        public string Name { get; set; }

        public double GetValue(int index)
        {
            if (index < Values.Count)
                return Values[index];

            if (index > Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (Values.Count > 0)
                Values.Add(Values[Values.Count - 1]);
            else
                Values.Add(0);

            var preValue = InputNode.GetValue(index);
            var value = _eq.EvaluateNext(preValue);

            Values[Values.Count - 1] = value;

            return Values[index];
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (index < Values.Count)
                return Values[index];

            if (index > Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (Values.Count > 0)
                Values.Add(Values[Values.Count - 1]);
            else
                Values.Add(0);

            var preValue = inputValues[0];
            var value = _eq.EvaluateNext(preValue);

            Values[Values.Count - 1] = value;

            return Values[index];
        }

        /// <inheritdoc />
        public int InputCount => 1;

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Filter system node" : $"{Name} (Filter)";
    }
}