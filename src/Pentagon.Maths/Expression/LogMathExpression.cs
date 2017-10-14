// -----------------------------------------------------------------------
//  <copyright file="LogMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    using System;
    using System.Globalization;
    using Pentagon.Extensions;

    public class LogMathExpression : MathExpression
    {
        public LogMathExpression(double @base, IMathExpression exp) : base(exp)
        {
            Base = @base;
        }

        public override double Value => GetValue(Math.Log(InnerExpression.Value, Base));

        public override string StringValue => $"log{(Base.EqualTo(10) ? "" : Base.SignificantFigures(3).ToString(CultureInfo.InvariantCulture))}({InnerExpression})";

        public double Base { get; }
        public IMathExpression InnerExpression => InnerExpressions?[0];
    }
}