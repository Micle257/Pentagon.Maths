// -----------------------------------------------------------------------
//  <copyright file="IirDigitalFilter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System.Collections.Generic;
    using System.Linq;

    public class IirDigitalFilter
    {
        public IirDigitalFilter(TransferFunction function)
        {
            Function = function;
        }

        public TransferFunction Function { get; }
        public List<double> InputSamples { get; } = new List<double>();
        public List<double> OutputSamples { get; } = new List<double>();

        public void SetInitialCondition(double c = double.NaN)
        {
            InputSamples.Clear();
            OutputSamples.Clear();
            if (!double.IsNaN(c))
                OutputSamples.Add(c);
        }

        public double[] ProcessSamples(double[] samples)
        {
            var res = new double[samples.Length];
            for (var i = 0; i < samples.Length; i++)
                res[i] = ProcessSample(samples[i]);
            return res;
        }

        public double ProcessSample(double sampleValue)
        {
            InputSamples.Add(sampleValue);

            var yn = ProcessInputs().Sum() - ProcessOutputs().Sum();
            OutputSamples.Add(yn);
            return yn;
        }

        List<double> ProcessOutputs()
        {
            var yValues = new List<double>();
            for (var i = 1; i < Function.Input.Parameters.Length; i++)
            {
                if (i > OutputSamples.Count)
                    break;
                var xc = Function.Input.Parameters[i];
                yValues.Add(xc * OutputSamples[OutputSamples.Count - i]);
            }
            return yValues;
        }

        List<double> ProcessInputs()
        {
            var xValues = new List<double>();
            for (var i = 0; i < Function.Output.Parameters.Length; i++)
            {
                if (i >= InputSamples.Count)
                    break;
                var yc = Function.Output.Parameters[i];
                xValues.Add(yc * InputSamples[InputSamples.Count - 1 - i]);
            }
            return xValues;
        }
    }
}