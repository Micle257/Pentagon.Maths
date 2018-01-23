namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Collections.Generic;

    public class InputSampleSystemNode : IInputSystemNode
    {
        public IList<double> Values { get; } = new  List<double>();

        public void Add(double value)
        {
            Values.Add(value);
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index)
        {
            return Values[index];
        }
    }
}