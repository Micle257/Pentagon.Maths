// -----------------------------------------------------------------------
//  <copyright file="MathExpr.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    public static class MathExpr
    {
        public static AddMathExpression Add(MathExpression e1, MathExpression e2, params MathExpression[] en) => new AddMathExpression(e1, e2, en);

        public static MultiplicationMathExpression Multiple(MathExpression e1, MathExpression e2, params MathExpression[] en) => new MultiplicationMathExpression(e1, e2, en);

        public static ValueMathExpression Value(double value) => new ValueMathExpression(value);

        public static UnknownMathExpression Unknown(string name) => new UnknownMathExpression(name);

        public static SinMathExpression Sin(MathExpression e1) => new SinMathExpression(e1);

        public static CosMathExpression Cos(MathExpression e) => new CosMathExpression(e);

        public static LogMathExpression Log(double @base, MathExpression e) => new LogMathExpression(@base, e);

        public static LnMathExpression Ln(MathExpression e) => new LnMathExpression(e);

        public static IMathExpression Cast(MathExpression mathExpression) => mathExpression;

        public static ExpMathExpression Exp(MathExpression baseExpression, MathExpression exponent) => new ExpMathExpression(baseExpression, exponent);
    }
}