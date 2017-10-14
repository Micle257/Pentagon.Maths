// -----------------------------------------------------------------------
//  <copyright file="FunctionTests.cs" company="The Pentagon">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Tests
{
    using System.Collections.Generic;
    using System.Numerics;
    using Expression;
    using Functions;
    using Helpers;
    using Pentagon.Extensions;
    using Quantities;
    using SignalProcessing;
    using Xunit;

    public class FunctionTests
    {
        [Fact]
        public void ShouldIntegrate()
        {
            var f = new Function(new UnknownMathExpression("x"));
            var intf = f.IntegrateApprox(new MathInterval(0,50), 1e-5);
            Assert.True(intf.EqualTo(1250, 1e-3));
        }

        [Fact]
        public void ShouldInitializeFunctionFromValues()
        {
            var f = new Function(new Dictionary<double, double> {{0, 1}, {1, 2}, {2, 3}});
            var valAtHalf = f.GetValue(0.5);
            Assert.Equal(1, valAtHalf);
            var valAt = f.GetValue(1.5);
            Assert.Equal(2, valAt);
        }

        [Fact]
        public void ShouldZTranform()
        {
            var s = new[] {1d, 2, 5};
            var f = new ZTranform(s);
            var r1 = f.Evaluate(Complex.One);
            Assert.Equal(new Complex(8, 0), r1);
            var r2 = f.Evaluate(Complex.ImaginaryOne);
            Assert.True(new Complex(-4, -2).EqualTo(r2));
        }

        [Fact]
        public void ShouldFunctionToDiscreateFunction()
        {
            var cf = new Function(a => 2 * a);
            var df = cf.ToDiscreteFunction((Frequency) 1, new MathInterval(-10d, 10d));
            Assert.Equal(-20, df.Samples[0]);
            Assert.Equal(-18, df.EvaluateSample(1));
            Assert.Equal(-20, df.EvaluateTime(0));
            Assert.Equal(-10, df.EvaluateTime(5));
            Assert.Equal(0, df.EvaluateTime(10));
        }
    }
}