// -----------------------------------------------------------------------
//  <copyright file="N.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Pentagon.Extensions;

    public class DelaySystemNode : ISingleInputNode
    {
        public int DelayLength { get; }

        public DelaySystemNode(int delayLength)
        {
            DelayLength = delayLength;
            for (int i = 0; i < DelayLength; i++)
            {
                _delayLine.Enqueue(0);
            }
        }
        
        Queue<double> _delayLine = new Queue<double>();

        /// <inheritdoc />
        public INode InputNode { get; private set; }

        bool _wasQueued;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            if (_wasQueued)
                return _delayLine.Peek();

            _wasQueued = true;

            var next = InputNode.GetValue(index);
            var value = _delayLine.Requeue(next);

            _wasQueued = false;
            return value;
        }

        /// <inheritdoc />
        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Delay: z^-{DelayLength}" : $"{Name} (Delay): z^-{DelayLength}";
    }
}