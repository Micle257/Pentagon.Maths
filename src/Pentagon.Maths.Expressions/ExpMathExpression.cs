// -----------------------------------------------------------------------
//  <copyright file="ExpMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions
{
    using System;
    using System.Linq;

    public class ExpMathExpression : MathExpression
    {
        public ExpMathExpression(IMathExpression exp) : base(new ValueMathExpression(Math.E), exp) { }

        public ExpMathExpression(IMathExpression baseExpression, IMathExpression exp) : base(baseExpression, exp) { }

        public override double Value => GetValue(Math.Pow(Base.Value, InnerExpression.Value));

        public override string StringValue
        {
            get { return InnerExpressions.Select(a => a.ToString()).Aggregate((e1, e2) => $"{e1} ^ {e2}"); }
        }

        public IMathExpression Base => InnerExpressions?[0];

        public IMathExpression InnerExpression => InnerExpressions?[1];
    }
}