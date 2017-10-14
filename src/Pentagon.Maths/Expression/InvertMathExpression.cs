// -----------------------------------------------------------------------
//  <copyright file="InvertMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    public class InvertMathExpression : MathExpression
    {
        public InvertMathExpression(IMathExpression exp) : base(exp) { }

        public override double Value => GetValue(1 / InnerExpression.Value);

        public override string StringValue => $"1/{InnerExpression}";

        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}