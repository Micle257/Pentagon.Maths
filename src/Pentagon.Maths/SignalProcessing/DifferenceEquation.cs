// -----------------------------------------------------------------------
//  <copyright file="ZTranform.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Expression;

    public class DifferenceEquation : ISystemDefinition
    {
        readonly DifferenceEquationCallback _function;
        SignalBuilder _inputSignal = new SignalBuilder();
        SignalBuilder _outputSignal = new SignalBuilder();
        bool _isEvaluating;

        public DifferenceEquation(Expression<DifferenceEquationCallback> function)
        {
            _function = function.Compile();
        }

        public void SetInitialCondition(Signal signal)
        {
            if (!_isEvaluating)
                _outputSignal.AddSignal(signal);
        }

        public double EvaluateNext(double x)
        {
            _isEvaluating = true;
            var y = _function(x, _inputSignal.GetRelativeSignal(), _outputSignal.GetRelativeSignal());

            _inputSignal.AddSample(x);
            _outputSignal.AddSample(y);

            return y;
        }

        public IEnumerable<double> EvaluateSignal(IEnumerable<double> samples)
        {
            foreach (var sample in samples)
            {
                yield return EvaluateNext(sample);
            }
        }
    }
}