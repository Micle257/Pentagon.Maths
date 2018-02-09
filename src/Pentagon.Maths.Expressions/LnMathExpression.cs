// -----------------------------------------------------------------------
//  <copyright file="LnMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    using System;

    public class LnMathExpression : MathExpression
    {
        public LnMathExpression(IMathExpression exp) : base(exp) { }

        public override double Value => GetValue(Math.Log(InnerExpression.Value));

        public override string StringValue => $"ln({InnerExpression})";

        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}