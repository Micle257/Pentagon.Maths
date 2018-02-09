// -----------------------------------------------------------------------
//  <copyright file="DivMathExpression.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    using System.Linq;

    public class DivMathExpression : MathExpression
    {
        public DivMathExpression(IMathExpression e1, IMathExpression e2, params IMathExpression[] nums) : this(new[] {e1, e2}.Concat(nums).ToArray()) { }

        internal DivMathExpression(params IMathExpression[] nums) : base(nums) { }

        public override double Value
        {
            get { return GetValue(InnerExpressions.Aggregate((a, b) => new ValueMathExpression(a.Value / b.Value)).Value); }
        }

        public override string StringValue
        {
            get { return InnerExpressions.Select(a => a.ToString()).Aggregate((e1, e2) => $"{e1} / {e2}"); }
        }
    }
}