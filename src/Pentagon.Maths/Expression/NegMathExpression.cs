// -----------------------------------------------------------------------
//  <copyright file="NegMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    public class NegMathExpression : MathExpression
    {
        public NegMathExpression(IMathExpression value) : base(value) { }

        public override double Value => GetValue(-InnerExpression.Value);

        public override string StringValue => $"-{InnerExpression}";

        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}