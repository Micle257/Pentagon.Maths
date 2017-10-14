// -----------------------------------------------------------------------
//  <copyright file="VectorTests.cs" company="The Pentagon">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Tests
{
    using Xunit;

    public class VectorTests
    {
        [Fact]
        public void ShouldAdd()
        {
            var a = new Vector(new[] {1d, 2, 3});
            var b = new Vector(new[] {1d, 2, 3, 4, 5});
            var c = new Vector(new[] {1d, 5});
            var sum = a + b + c;
            Assert.Equal(new[] {3d, 9, 6, 4, 5}, sum.Values);
        }
    }
}