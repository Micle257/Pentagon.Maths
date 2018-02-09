// -----------------------------------------------------------------------
//  <copyright file="ValueMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    public class ValueMathExpression : MathExpression
    {
        public ValueMathExpression(double value)
        {
            Value = value;
            IsConstant = true;
        }

        public override double Value { get; }

        public override string StringValue => $"{Value}";

        #region Operators

        public static implicit operator double(ValueMathExpression value) => value.Value;

        #endregion
    }
}