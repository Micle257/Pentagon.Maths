namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class SumSystemNode : IMultiInputNode
    {
        /// <inheritdoc />
        public string Name { get; set; }
        
        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var sum = 0d;
            foreach (var node in inputValues)
            {
                sum += node;
            }
            return sum;
        }
        
        /// <inheritdoc />
        public override string ToString() => Name == null ? $"Sum system node" : $"{Name} (Sum)";
    }
}