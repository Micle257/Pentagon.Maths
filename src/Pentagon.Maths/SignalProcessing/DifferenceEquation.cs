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

    public class DifferenceEquation
    {
        readonly DifferenceEquationCallback _function;
        SignalBuilder _inputSignal = new SignalBuilder();
        SignalBuilder _outputSignal = new SignalBuilder();
        bool _isEvaluating;

        public Expression<DifferenceEquationCallback> Expression { get; }

        public DifferenceEquation(Expression<DifferenceEquationCallback> function)
        {
            Expression = function;
            _function = function.Compile();
        }

        public void SetInitialCondition(Signal signal = null)
        {
            _isEvaluating = false;
            _inputSignal = new SignalBuilder();
            _outputSignal = new SignalBuilder();
            if (signal != null)
                _outputSignal.AddSignal(signal);
        }

        public double EvaluateNext(double x)
        {
            _isEvaluating = true;
            _inputSignal.AddSample(x);
            _outputSignal.AddSample(_outputSignal.RelativeSignal[0]);

            var y = _function(_inputSignal.RelativeSignal, _outputSignal.RelativeSignal);

            _outputSignal.SetLastSample(y);

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