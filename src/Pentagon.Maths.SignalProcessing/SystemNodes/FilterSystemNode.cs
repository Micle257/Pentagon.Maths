// -----------------------------------------------------------------------
//  <copyright file="FilterSystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Abstractions;

    public class FilterSystemNode : IFilterSystemNode, ISingleInputNode, IMemoryNode
    {
        DifferenceEquation _eq;

        bool _wasEvaluated;

        public FilterSystemNode(IEnumerable<double> numeratorCoefficients, IEnumerable<double> denumeratorCoefficients)
        {
            _eq = new DifferenceEquation(numeratorCoefficients, denumeratorCoefficients);
        }

        public FilterSystemNode(ISystemFunction system)
        {
            _eq = system as DifferenceEquation ?? new DifferenceEquation(system.Coefficients);
        }

        public FilterSystemNode(Expression<Func<RelativeSignal, RelativeSignal, double>> equationCallback)
        {
            _eq = DifferenceEquation.FromExpression(equationCallback);
        }

        public ISystemFunction System { get; }

        /// <inheritdoc />
        public double LastValue => _eq.LastValue;

        /// <inheritdoc />
        public string Name { get; set; }

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
        public override string ToString() => Name == null ? $"Filter system node" : $"{Name} (Filter)";

        public void Reset()
        {
            _eq = _eq.CopyInitial();
        }
    }
}