namespace Pentagon.Maths {
    using JetBrains.Annotations;

    public class RelativeSignal
    {
        [NotNull]
        readonly Signal _signal;

        public RelativeSignal([NotNull] Signal signal)
        {
            Require.NotNull(() => signal);
            _signal = signal;
        }

        public double this[int delay]
        {
            get
            {
                if (_signal.Length + delay < 0 || _signal.Length + delay >= _signal.Length)
                    return 0;
                else
                    return _signal[_signal.Length + delay];
            }
        }
    }
}