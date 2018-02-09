namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    using System;
    using System.Diagnostics;

    public class FunctionSystemNode : ISingleInputNode
    {
        readonly Func<double, double> _function;
        
        public FunctionSystemNode(Func<double,double> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Name { get; set; }
        
        /// <inheritdoc />
        public double GetValue(int index, params double[] inputValues)
        {
            var value = inputValues[0];
            var output = _function(value);

            return output;
        }
    }
}