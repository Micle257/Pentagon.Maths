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
            // y[n] = x[n] - 0.5x[n-1] + 0.877y[n-1]
            
            var man = new SystemConnectionManager<Waveguide>();

            man.SetupSystem(new Waveguide());

            var val = new List<double>();
            for (int i = 0; i < 48000; i++)
            {
                val.Add(man.GetValue(i));
            }
        }

        public class Waveguide : INodeSystem
        {
            public SumSystemNode TopSum { get; } = new SumSystemNode {Name = "Top sum"};

            public SumSystemNode BottomSum { get; } = new SumSystemNode { Name = "Bottom sum" };

            public FilterSystemNode Filter { get; } = new FilterSystemNode((x,y) => 0.8*x[0] + 0.1*x[-1] - 0.2*y[-1]) { Name = "Filter" };

            public DelaySystemNode TopRightDelay { get; } = new DelaySystemNode(5) { Name = "Top right" };

            public DelaySystemNode BottomRightDelay { get; } = new DelaySystemNode(5) { Name = "Bottom right" };

            public DelaySystemNode TopLeftDelay { get; } = new DelaySystemNode(2) { Name = "Top left" };

            public DelaySystemNode BottomLeftDelay { get; } = new DelaySystemNode(2) { Name = "Bottom left" };

            public FactorSystemNode Factor { get; } = new FactorSystemNode(0.2) { Name = "Factor" };

            public StepImpulseInputSystemNode Input { get; } = new StepImpulseInputSystemNode { Name = "Input" };

            /// <inheritdoc />
            public INode Output => TopRightDelay;

            /// <inheritdoc />
            public void ConfigureConnections(IConnectionBuilder builder)
            {
                builder.Connect(TopRightDelay, TopSum)
                       .Connect(TopSum, TopLeftDelay, Factor)
                       .Connect(TopLeftDelay, BottomLeftDelay)
                       .Connect(BottomLeftDelay, BottomSum)
                       .Connect(BottomSum, BottomRightDelay)
                       .Connect(BottomRightDelay, Filter)
                       .Connect(Filter, TopRightDelay);

                builder.Connect(BottomSum, Factor);

                builder.Connect(Factor, Input);
            }
        }

        public class System : INodeSystem
        {
            public StepImpulseInputSystemNode Input { get; } = new StepImpulseInputSystemNode { Name = "Input"};

            public SumSystemNode Sum { get; } = new SumSystemNode {Name = "Output sum"};

            public DelaySystemNode InputDelay { get; } = new DelaySystemNode(1) {Name = "x[n-1]"};

            public DelaySystemNode OutputDelay { get; } = new DelaySystemNode(1) { Name = "y[n-1]" };

            public FactorSystemNode OutputFactor { get; } = new FactorSystemNode(0.877) {Name = "Factor for output delay"};

            public FactorSystemNode InputOneFactor { get; } = new FactorSystemNode(-.5) { Name = "Factor for input delay" };

            /// <inheritdoc />
            public INode Output => Sum;

            public void ConfigureConnections(IConnectionBuilder builder)
            {
                builder.Connect(Sum, Input, InputOneFactor, OutputFactor);
                builder.Connect(InputDelay, Input)
                 .Connect(OutputDelay, Sum)
                 .Connect(OutputFactor, OutputDelay)
                       .Connect(InputOneFactor, InputDelay);
            }
        }
    }
}
