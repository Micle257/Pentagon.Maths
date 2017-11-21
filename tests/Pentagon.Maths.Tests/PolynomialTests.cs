// -----------------------------------------------------------------------
//  <copyright file="PolynomialTests.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Maths.Tests
{
    using SignalProcessing;
    using Xunit;

    public class PolynomialFractionTests
    {
        [Fact]
        public void InTermsOf_ReturnsCorectValue()
        {
            var p1 = new Polynomial(new double[] {1,2});
            var p2 = new Polynomial(new double[] { 5,3,1 });

            var p3 = new Polynomial(new double[] { 5,9,1 });
            var p4 = new Polynomial(new double[] {2,3 });

            var fraction1 = new PolynomialFraction(p1,p2);
            var fraction2 = new PolynomialFraction(p3, p4);

            var terms = fraction1.InTermsOf(fraction2);

            Assert.Equal(new double[] {2,7,6}, terms.Numerator.Coefficients);
            Assert.Equal(new double[] { 10,21,11,3 }, terms.Denumerator.Coefficients);
        }

        [Fact]
        public void Add_ReturnsCorectValue()
        {
            var p1 = new Polynomial(new double[] { 1, 2 });
            var p2 = new Polynomial(new double[] { 5, 3, 1 });

            var p3 = new Polynomial(new double[] { 5, 9, 1 });
            var p4 = new Polynomial(new double[] { 2, 3 });

            var fraction1 = new PolynomialFraction(p1, p2);
            var fraction2 = new PolynomialFraction(p3, p4);

            var result = fraction1.Add(fraction2);

            Assert.Equal(new double[] {27,67,43,12,1 }, result.Numerator.Coefficients);
            Assert.Equal(new double[] { 10, 21, 11, 3 }, result.Denumerator.Coefficients);
        }

        [Fact]
        public void Multiple_ReturnsCorectValue()
        {
            var p1 = new Polynomial(new double[] { 1, 2 });
            var p2 = new Polynomial(new double[] { 5, 3, 1 });

            var p3 = new Polynomial(new double[] { 5, 9, 1 });
            var p4 = new Polynomial(new double[] { 2, 3 });

            var fraction1 = new PolynomialFraction(p1, p2);
            var fraction2 = new PolynomialFraction(p3, p4);

            var result = fraction1.Multiple(fraction2);

            Assert.Equal(new double[] { 5,19,19,2 }, result.Numerator.Coefficients);
            Assert.Equal(new double[] { 10,21,11,3 }, result.Denumerator.Coefficients);
        }
    }

    public class PolynomialTests
    {
        [Fact]
        public void Add_AddPolynomials()
        {
            var p1 = new Polynomial(new [] {1d,5,3});
            var p2 = new Polynomial(new [] {2d, 9,7, 1,3});

            var add = Polynomial.Add(p1, p2);

            Assert.Equal(new [] {3d, 14, 10, 1, 3}, add.Coefficients);
        }

        [Theory]
        [InlineData(new [] {1d,5}, new [] {2d,9,7}, new[] { 2d, 19, 52, 35 })]
        [InlineData(new[] { 5d, 3,-4 }, new[] { 7d, -2, -1,1,3 }, new[] { 35d, 11, -39, 10,22,5,-12 })]
        public void Multiple_MultiplePolynomials(double[] c1, double[] c2, double[] expected)
        {
            var p1 = new Polynomial(c1);
            var p2 = new Polynomial(c2);

            var add = Polynomial.Multiple(p1, p2);

            Assert.Equal(expected, add.Coefficients);
        }
    }
}