using System;

namespace Pentagon.Maths.SignalProcessing.Demo
{
    using System.Collections.Generic;
    using SystemNodes;
    using Quantities;
    using SignalSources;

    class Program
    {
        static void Main(string[] args)
        {
            //system H(z)=(1+z^-1)/(1-0.877z^-1)
            // y[n] = x[n] + x[n-1] + 0.877y[n-1]

            var s1 = GetSeperateSystem();
            var s2 = GetSeperateSystem();

            var o1 = new List<double>();
            var oH = new List<double>();

            var sf = 192000;
            
            var sin = new SinSignalSource(new Frequency(sf), new Frequency(100));
            var sinH = new SinSignalSource(new Frequency(sf), new Frequency(10000));
            
            for (int i = 0; i < sf; i++)
            {
                s1.input.Add(sin.GetValueAt(i));
                o1.Add(s1.output.GetValue(i));

                s2.input.Add(sinH.GetValueAt(i));
                oH.Add(s2.output.GetValue(i));
            }
        }

        static (InputSampleSystemNode input, INode output) GetSeperateSystem()
        {
            var input = new InputSampleSystemNode();
            var sum = new SumSystemNode();
            var inputDelay = new DelaySystemNode(1);
            var outputDelay = new DelaySystemNode(1);
            var factor = new FactorSystemNode(0.877);

            sum.AddInputNode(input);
            sum.AddInputNode(inputDelay);
            sum.AddInputNode(factor);
            inputDelay.SetInputNode(input);
            outputDelay.SetInputNode(sum);
            factor.SetInputNode(outputDelay);

            return (input, sum);
        }
    }
}
