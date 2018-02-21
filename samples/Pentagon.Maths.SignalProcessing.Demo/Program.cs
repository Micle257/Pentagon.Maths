using System;

namespace Pentagon.Maths.SignalProcessing.Demo
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SystemNodes;
    using Quantities;
    using SignalSources;

    class Program
    {
       static SystemConnectionManager<System> _manager = new SystemConnectionManager<System>();

        static void Main(string[] args)
        {
            _manager.SetupSystem(new System());
            
            Measure(() =>
                    {
                        var val = new List<double>();
                        for (var i = 0; i < 48000; i++)
                        {
                            val.Add(_manager.GetValue(i));
                        }
                    });
        }

        static void Measure(Action act)
        {
            var stop = new Stopwatch();
            var span = new List<TimeSpan>();

            for (var k = 0; k < 100; k++)
            {
                stop.Start();

                act();

                stop.Stop();
                span.Add(stop.Elapsed);
                stop.Reset();
            }

            var average = span.Average(a => a.TotalSeconds);
        }

        public class Waveguide : INodeSystem
        {
            public SumSystemNode TopSum { get; } = new SumSystemNode { Name = "Top sum" };

            public SumSystemNode BottomSum { get; } = new SumSystemNode { Name = "Bottom sum" };

            public FilterSystemNode Filter { get; } = new FilterSystemNode((x, y) => 0.8 * x[0] + 0.1 * x[-1] - 0.2 * y[-1]) { Name = "Filter" };

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

            /// <inheritdoc />
            public void Initialize()
            {

            }
        }

        public class System : INodeSystem
        {
            public StepImpulseInputSystemNode Input1 { get; } = new StepImpulseInputSystemNode { Name = "Input1" };

            public StepImpulseInputSystemNode Input2 { get; } = new StepImpulseInputSystemNode { Name = "Input2" };

            public SumSystemNode LeftSum { get; } = new SumSystemNode { Name = "Sum" };

            public SumSystemNode RightSum { get; } = new SumSystemNode { Name = "Output sum" };

            public DelaySystemNode Delay1 { get; } = new DelaySystemNode(1) { Name = "Delay1" };

            public DelaySystemNode Delay2 { get; } = new DelaySystemNode(2) { Name = "Delay2" };

            public FactorSystemNode OutputFactor { get; } = new FactorSystemNode(.2) { Name = "Factor for output" };

            public FactorSystemNode InputFactor { get; } = new FactorSystemNode(0.3) { Name = "Factor for input" };

            public FactorSystemNode BackFactor { get; } = new FactorSystemNode(0.1) { Name = "Factor for input" };

            public FilterSystemNode Filter { get; } = new FilterSystemNode((x, y) => 0.8 * x[0] + 0.1 * x[-1]);

            /// <inheritdoc />
            public INode Output => RightSum;

            public void ConfigureConnections(IConnectionBuilder builder)
            {
                builder.Connect(RightSum, InputFactor, OutputFactor)
                       .Connect(InputFactor, Input2)
                       .Connect(LeftSum, Input1, BackFactor, InputFactor, Delay2)
                       .Connect(BackFactor, Delay1)
                       .Connect(OutputFactor, LeftSum)
                       .Connect(Delay1, LeftSum)
                       .Connect(Delay2, RightSum);
            }

            /// <inheritdoc />
            public void Initialize()
            {

            }
        }

        public class FilterSystem : INodeSystem
        {
            public IInputSystemNode Input { get; } = new StepImpulseInputSystemNode(); 

            public FilterSystemNode Loss { get; } = new FilterSystemNode(new [] {0, -0.996},new [] {1, -0.001}) {Name = "Loss"};

            public FilterSystemNode Dispersion1 { get; } = new FilterSystemNode(new[] { -0.3, 1 }, new[] { 1, -0.3 }) { Name = "Disp1" };

            public FilterSystemNode Dispersion2 { get; } = new FilterSystemNode(new[] { -0.3, 1 }, new[] { 1, -0.3 }) { Name = "Disp2" };

            public FilterSystemNode Tuning { get; } = new FilterSystemNode(new[] { -0.141848762041005, 1 }, new[] { 1, -0.141848762041005 }) { Name = "Tun" };

            /// <inheritdoc />
            public INode Output => Loss;

            /// <inheritdoc />
            public void ConfigureConnections(IConnectionBuilder builder)
            {
                builder.Connect(Dispersion1, Input)
                       .Connect(Dispersion2, Dispersion1)
                       .Connect(Tuning, Dispersion2)
                       .Connect(Loss, Tuning);
            }

            /// <inheritdoc />
            public void Initialize()
            {
               
            }
        }


    }
}
