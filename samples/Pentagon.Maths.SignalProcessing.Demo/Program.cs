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

            var man = new SystemConnectionManager<System>();

            man.SetupSystem(new System());
            
            man.InitializeOutputNode(b => b.Sum);

            var val = new List<double>();
            for (int i = 0; i < 48000*25; i++)
            {
                val.Add(man.GetValue(i));
            }
        }

        public class System : INodeSystem
        {
            public StepImpulseInputSystemNode Input { get; } = new StepImpulseInputSystemNode { Name = "Input"};

            public SumSystemNode Sum { get; } = new SumSystemNode {Name = "Output sum"};

            public DelaySystemNode InputDelay { get; } = new DelaySystemNode(1) {Name = "x[n-1]"};

            public DelaySystemNode OutputDelay { get; } = new DelaySystemNode(1) { Name = "y[n-1]" };

            public FactorSystemNode Factor { get; } = new FactorSystemNode(0.877) {Name = "Factor for output delay"};

            public void ConfigureConnections(IConnectionBuilder builder)
            {
                builder.Connect(Sum, Input, InputDelay, Factor);
                builder.Connect(InputDelay, Input)
                 .Connect(OutputDelay, Sum)
                 .Connect(Factor, OutputDelay);
            }
        }
    }
}
