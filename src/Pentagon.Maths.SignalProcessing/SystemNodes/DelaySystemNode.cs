// -----------------------------------------------------------------------
//  <copyright file="DelaySystemNode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Generic;
    using Abstractions;

    public class DelaySystemNode : IDelaySystemNode, ISingleInputNode, IMemoryNode
    {
        readonly Queue<double> _delayLine = new Queue<double>();

        int _lastIndex = -1;

        public DelaySystemNode(int delayLength)
        {
            DelayLength = delayLength;
            for (var i = 0; i < DelayLength; i++)
                _delayLine.Enqueue(0);
        }

        public int DelayLength { get; }

        /// <inheritdoc />
        public double LastValue => _delayLine.Peek();

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            if (index == _lastIndex)
                return _delayLine.Peek();

            var next = inputValues[0];
            var value = _delayLine.Dequeue();
            _delayLine.Enqueue(next);

            _lastIndex = index;

            return value;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Delay: z^-{DelayLength}" : $"{Name} (Delay): z^-{DelayLength}";

        public void Reset()
        {
            for (var i = 0; i < DelayLength; i++)
            {
                _delayLine.Dequeue();
                _delayLine.Enqueue(0);
            }
        }
    }
}