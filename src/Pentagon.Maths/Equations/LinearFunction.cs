// -----------------------------------------------------------------------
//  <copyright file="LinearFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Equations
{
    using System.Collections.Generic;
    using System.Linq;
    using Expression;
    using Functions;

    public class LinearFunction : Function
    {
        public LinearFunction(double a, double b)
            : base(new AddMathExpression(new MultiplicationMathExpression(new ValueMathExpression(a), new UnknownMathExpression("x")), new ValueMathExpression(b))) { }

        public static LinearFunction FromTwoPoints(MathPoint a, MathPoint b)
        {
            var ac = (b.Y - a.Y) / (b.X - a.X);
            var bc = a.Y - ac * a.X;
            return new LinearFunction(ac, bc);
        }

        public static LinearFunction FromPointSlope(MathPoint startPoint, double slope)
        {
            var bc = startPoint.Y - slope * startPoint.X;
            return new LinearFunction(slope, bc);
        }

        public static LinearFunction FromLinearRegression(MathPoint p1, MathPoint p2, params MathPoint[] pn)
        {
            double a;
            double b;
            double r;
            Regression.LinearRegression(new List<MathPoint> {p1, p2}.Concat(pn), out r, out b, out a);
            return new LinearFunction(a, b);
        }
    }
}