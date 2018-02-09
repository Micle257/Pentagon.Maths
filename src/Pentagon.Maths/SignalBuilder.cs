// -----------------------------------------------------------------------
//  <copyright file="SignalBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System.Collections.Generic;
    using System.Linq;

    public class SignalBuilder
    {
        readonly IList<double> _values = new List<double>();

        public SignalBuilder()
        {
            RelativeSignal = new RelativeSignalold(this);
        }

        public IReadOnlyList<double> Values => _values as IReadOnlyList<double>;

        public RelativeSignalold RelativeSignal { get; }

        public void AddSample(double sample)
        {
            _values.Add(sample);
        }

        public Signal GetSignal() => new Signal(Values);

        public void AddSignal(Signal signal)
        {
            foreach (var d in signal)
                AddSample(d);
        }

        public void SetLastSample(double value)
        {
            if (_values.Any())
                _values[_values.Count - 1] = value;
        }
    }
}