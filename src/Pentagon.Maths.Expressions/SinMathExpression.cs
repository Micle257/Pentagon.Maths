// -----------------------------------------------------------------------
//  <copyright file="SinMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions
{
    using System;

    public class SinMathExpression : MathExpression
    {
        public SinMathExpression(IMathExpression exp) : base(exp) { }

        public override double Value => GetValue(Math.Sin(InnerExpression.Value));

        public override string StringValue => $"sin{InnerExpression}";

        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}