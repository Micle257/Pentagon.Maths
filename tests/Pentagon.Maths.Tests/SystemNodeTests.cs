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
            var sum = new SumSystemNode();
            var f = new FactorSystemNode(0.2);
            var input = new InputFunctionSystemNode(new InfiniteDiscreteFunction(a => a, new Frequency(100)));

            delay.SetInputNode(sum);
            f.SetInputNode(delay);
            sum.AddInputNode(input);
            sum.AddInputNode(f);

            var result = new List<double>();
            for (int i = 0; i <150; i++)
            {
                result.Add(delay.GetValue(i));
            }
        }
    }
}