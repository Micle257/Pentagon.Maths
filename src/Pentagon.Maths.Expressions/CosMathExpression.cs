// -----------------------------------------------------------------------
//  <copyright file="CosMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    using System;

    public class CosMathExpression : MathExpression
    {
        public CosMathExpression(IMathExpression exp) : base(exp) { }

        public override double Value => GetValue(Math.Cos(InnerExpression.Value));

        public override string StringValue => $"cos{InnerExpression}";

        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}