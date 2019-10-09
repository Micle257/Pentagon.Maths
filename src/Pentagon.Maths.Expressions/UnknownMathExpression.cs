// -----------------------------------------------------------------------
//  <copyright file="UnknownMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions
{
    using System.Collections.Generic;

    public class UnknownMathExpression : MathExpression
    {
        double _value;

        public UnknownMathExpression(string name)
        {
            Name = name;
            IsValuable = false;
            InnerExpressions = new List<IMathExpression>();
        }

        public static UnknownMathExpression X => new UnknownMathExpression(name: "x");

        public override double Value => GetValue(_value);

        public override string StringValue => Name;

        public string Name { get; }

        public void SetUnknownValue(double value)
        {
            IsValuable = true;
            _value = value;
        }

        public void ResetUnknown()
        {
            IsValuable = false;
        }
    }
}