namespace Pentagon.Maths {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Pentagon.Extensions;

	public struct RelativeSignal
	{
		public int Size { get; }
		
		List<double> _values;

		public RelativeSignal(int size)
		{
			Size = size;

			_values = new List<double>(Enumerable.Repeat(0d, size));
		}

		public void AddSample(double value)
		{
			//_queue.Requeue(value);
			_values.RemoveAt(0);
			_values.Add(value);
		}

		public double this[int delay]
		{
			get
			{
				if (delay > 0)
					throw new ArgumentOutOfRangeException(nameof(delay));

				if (-delay > Size)
					throw new ArgumentOutOfRangeException(nameof(delay));

				return _values[Size + delay - 1];
			}
		}

		public void SetLastSample(double value)
		{
			_values[_values.Count - 1] = value;
		}
	}

	public class RelativeSignalold
	{
		[NotNull]
		readonly SignalBuilder _signal;

		public RelativeSignalold([NotNull] SignalBuilder signal)
		{
			_signal = signal ?? throw new ArgumentNullException(nameof(signal));
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