namespace Pentagon.Maths.Tests {
    using System.Collections.Generic;
    using Functions;
    using Quantities;
    using SignalProcessing.SystemNodes;

    public class SystemNodeTests
    {
        [Xunit.FactAttribute]
        public void FactMethodName_Scenario_ExpectedBehavior()
        {
            var delay = new DelaySystemNode(1);
            var backDelay = new DelaySystemNode(2);
            var sum1 = new SumSystemNode();
            var sum2 = new SumSystemNode();
            var f = new FactorSystemNode(0.5);
            var input = new StepImpulseInputSystemNode();

            sum1.AddInputNode(input);
            delay.SetInputNode(sum1);
            sum2.AddInputNode(delay);
            sum2.AddInputNode(input);
            backDelay.SetInputNode(delay);
            f.SetInputNode(backDelay);
            sum1.AddInputNode(f);

            var result = new List<double>();
            for (int i = 0; i <150; i++)
            {
                result.Add(sum2.GetValue(i));
            }
        }
    }
}