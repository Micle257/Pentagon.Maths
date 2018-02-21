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

    public class EmptySystemNode : INode, ISingleInputNode
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            return inputValues[0];
        }
    }

    public class DelayOutputSystemNode : ISingleInputNode
    {
        public IDelaySystemNode Delay { get; }

        /// <inheritdoc />
        public string Name { get; set; }

        public DelayOutputSystemNode(IDelaySystemNode delay)
        {
            Delay = delay;
        }

        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            return Delay.LastValue;
        }
    }
    
    public class DelaySystemNode : IDelaySystemNode, ISingleInputNode, IMemoryNode
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
        public string Name { get; set; }

        /// <inheritdoc />
        public double LastValue =>  _delayLine.Peek();
        
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

        int _lastIndex = -1;
        
        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Delay: z^-{DelayLength}" : $"{Name} (Delay): z^-{DelayLength}";
    }
}