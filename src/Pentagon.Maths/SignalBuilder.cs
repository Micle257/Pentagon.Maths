namespace Pentagon.Maths {
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class SignalBuilder
    {
        RelativeSignal _relativeSignal;

        public IReadOnlyList<double> Values => _values as IReadOnlyList<double>;

        IList<double> _values = new List<double>();

        public SignalBuilder()
        {
            _relativeSignal = new RelativeSignal(this);
        }

        public void AddSample(double sample)
        {
            _values.Add(sample);
        }

        public Signal GetSignal() => new Signal(Values);

        public RelativeSignal RelativeSignal => _relativeSignal;

        public void AddSignal(Signal signal)
        {
            foreach (var d in signal)
            {
                AddSample(d);
            }
        }
    }
}