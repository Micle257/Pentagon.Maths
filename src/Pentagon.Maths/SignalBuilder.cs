namespace Pentagon.Maths {
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class SignalBuilder
    {
        ICollection<double> _values = new List<double>();

        public void AddSample(double sample)
        {
            _values.Add(sample);
        }

        [NotNull]
        public Signal GetSignal() => new Signal(_values);

        public RelativeSignal GetRelativeSignal()
        {
            return new RelativeSignal(GetSignal());
        }

        public void AddSignal(Signal signal)
        {
            foreach (var d in signal)
            {
                AddSample(d);
            }
        }
    }
}