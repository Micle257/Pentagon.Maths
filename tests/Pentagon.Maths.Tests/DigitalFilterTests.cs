// -----------------------------------------------------------------------
//  <copyright file="DigitalFilterTests.cs" company="The Pentagon">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Tests
{
    using System.Collections.Generic;
    using Expression;
    using Functions;
    using Helpers;
    using Quantities;
    using Units;
    using Xunit;

    public class DigitalFilterTests
    {
        [Theory]
        [InlineData(new[] {1d, 3, 9, 10, 15})]
        public void ShouldProcessSample(double[] samples)
        {
            var outputZ = new ZTranform(new[] {1d, 2, 1});
            var inputZ = new ZTranform(new[] {1d, 1 / 3d, -1 / 4d});
            outputZ.ToString();
            var f = new TransferFunction(outputZ, inputZ);
            var filter = new LinearTimeInvariantSystem(f);
            var yn = new List<double>();
            foreach (var sample in samples)
                yn.Add(filter.ProcessSample(sample));
        }

        [Fact]
        public void ShouldStepResponse()
        {
            var outputZ = new ZTranform(new[] {5d});
            var inputZ = new ZTranform(new[] {5d});
            var f = new TransferFunction(outputZ, inputZ);
            var filter = new LinearTimeInvariantSystem(f);
            var yn = new List<double>();
            var step = InfiniteDiscreteFunction.StepFunction(new Frequency(48000));
            var samples = step.EvaluateSamples(new Range<int>(0, 500));
            foreach (var sample in samples)
                yn.Add(filter.ProcessSample(sample));
        }

        [Fact]
        public void ShouldDifferenceEq()
        {
            var outputZ1 = new ZTranform(new[] { 1d, 2, 1 });
            var inputZ1 = new ZTranform(new[] { 1d, 1 / 3d, -1 / 4d });
            var outputZ = new ZTranform(new[] {1d});
            var inputZ = new ZTranform(new[] {1d, 0.92});
            var f = new TransferFunction(outputZ, inputZ);
            var fs = new TransferFunction(outputZ1, inputZ1);
            var sss = f + fs;
            var de = f.ToDifferenceEquation();
            var signal = new Function(MathExpr.Sin(Function.IndependentName * (ValueMathExpression) new Frequency(10000).ConvertUnit(new AngularSpeed()).Value)).ToDiscreteFunction((Frequency) 48000);
            var ss = de.EvaluateSignal(signal.EvaluateSamples(new Range<int>(0, 100)));
        }

        [Fact]
        public void ShouldSystemGetImpulseResponse()
        {
            var outputZ = new ZTranform(new[] {1d});
            var inputZ = new ZTranform(new[] {1d, 0.92});
            var f = new TransferFunction(outputZ, inputZ);
            var yn = new List<double>();
            var cs = new LinearTimeInvariantSystem(f);
            var d = cs.GetImpulseResponse(new Frequency(48000));
            var ss = d.EvaluateSamples(new Range<int>(0, 100));
        }
    }
}