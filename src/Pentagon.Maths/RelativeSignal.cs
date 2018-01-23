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
                var index = (_signal.Values.Count - 1) + delay;
                if (index < 0 || index >= _signal.Values.Count)
                    return 0;
                else
                    return _signal.Values[index];
            }
        }
    }
}