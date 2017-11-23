namespace Pentagon.Maths {
    using JetBrains.Annotations;

    public class RelativeSignal
    {
        [NotNull]
        readonly SignalBuilder _signal;

        public RelativeSignal([NotNull] SignalBuilder signal)
        {
            Require.NotNull(() => signal);
            _signal = signal;
        }

        public double this[int delay]
        {
            get
            {
                if (_signal.Values.Count + delay < 0 || _signal.Values.Count + delay >= _signal.Values.Count)
                    return 0;
                else
                    return _signal.Values[_signal.Values.Count + delay];
            }
        }
    }
}