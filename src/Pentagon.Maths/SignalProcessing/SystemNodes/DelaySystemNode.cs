// -----------------------------------------------------------------------
//  <copyright file="N.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System;
    using System.Collections.Generic;

    public class DelaySystemNode : IMemoryNode, ISingleInputNode
    {
        public int DelayLength { get; }

        public DelaySystemNode(int delayLength)
        {
            DelayLength = delayLength;
            for (int i = 0; i < DelayLength; i++)
            {
                Values.Add(0);
            }
        }

        public IList<double> Values { get; } = new List<double>();
        public INode InputNode { get; private set; }

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
            //if (index < DelayLength)
            //    return 0;

            //var value = Values[index - DelayLength];
            //return value;
        }

        public void SetInputNode(INode node)
        {
            InputNode = node;
        }
    }
}