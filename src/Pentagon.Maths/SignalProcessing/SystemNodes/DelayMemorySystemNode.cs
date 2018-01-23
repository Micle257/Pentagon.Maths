namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;

    public class DelayMemorySystemNode : IMemoryNode, ISingleInputNode
    {
        public int DelayLength { get; }

        public DelayMemorySystemNode(int delayLength)
        {
            DelayLength = delayLength;
            for (int i = 0; i < DelayLength; i++)
            {
                Values.Add(0);
            }
        }

        /// <inheritdoc />
        public IList<double> Values { get; } = new List<double>();

        /// <inheritdoc />
        public INode InputNode { get; private set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            if (index < Values.Count - DelayLength)
                return Values[index];

            if (index > Values.Count - DelayLength)
                throw new ArgumentOutOfRangeException(nameof(index));

            Values.Add(Values[Values.Count - DelayLength]);
            var preValue = InputNode.GetValue(index);

            Values[Values.Count - 1] = preValue;

            return Values[index];
        }
        
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public void SetInputNode(INode node)
        {
            InputNode = node;
        }

        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Delay: z^-{DelayLength}" : $"{Name} (Delay): z^-{DelayLength}";
    }
}