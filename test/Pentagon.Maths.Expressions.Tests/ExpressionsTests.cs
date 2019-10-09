// -----------------------------------------------------------------------
//  <copyright file="ExpressionsTests.cs" company="The Pentagon">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Tests
{
    using System;
    using Expressions.String;
    using Xunit;

    public class ExpressionsTests
    {
        [Fact]
        public void TestMethod1()
        {
            var str1 = new StringExpression("(2)*(5)");
            var e1 = str1.Compute();
            Assert.Equal(10, e1.Value);
            //str1 = new StringExpression("ln(3)");
            //e1 = str1.Compute();
            //Assert.True(e1.Value.EqualTo(Math.Log(3)));
            str1 = new StringExpression("cos(3.14)");
            e1 = str1.Compute();
            Assert.Equal(Math.Cos(3.14), e1.Value);
        }
    }
}