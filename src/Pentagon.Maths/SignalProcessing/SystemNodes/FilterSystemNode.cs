namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;

    public class FilterSystemNode : IMemoryNode, ISingleInputNode
    {
        DifferenceEquation _eq;
        public IList<double> Values { get; } = new List<double>();

        public INode InputNode { get; private set; }

        public FilterSystemNode(DifferenceEquationCallback equationCallback)
        {
            _eq = new DifferenceEquation(equationCallback);
        }

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

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }
    }
}