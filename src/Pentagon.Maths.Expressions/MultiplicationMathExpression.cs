// -----------------------------------------------------------------------
//  <copyright file="MultiplicationMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions
{
    using System.Linq;

    public class MultiplicationMathExpression : MathExpression
    {
        public MultiplicationMathExpression(IMathExpression e1, IMathExpression e2, params IMathExpression[] nums) : this(new[] {e1, e2}.Concat(nums).ToArray()) { }

        internal MultiplicationMathExpression(params IMathExpression[] nums) : base(nums) { }

        public override double Value
        {
            get { return GetValue(InnerExpressions.Aggregate((a, b) => new ValueMathExpression(a.Value * b.Value)).Value); }
        }

        public override string StringValue
        {
            get { return InnerExpressions.Select(a => a.ToString()).Aggregate((e1, e2) => $"{e1} * {e2}"); }
        }
    }
}