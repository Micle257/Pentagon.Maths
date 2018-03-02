// -----------------------------------------------------------------------
//  <copyright file="MathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class MathExpression : IMathExpression
    {
        protected MathExpression() { }

        protected MathExpression(params IMathExpression[] nums) : this()
        {
            InnerExpressions = new List<IMathExpression>(nums);
        }

        public abstract string StringValue { get; }

        public IEnumerable<IMathExpression> AllInnerExpressions
        {
            get
            {
                yield return this;
                foreach (var exp in InnerExpressions)
                {
                    foreach (var expAllInnerExpression in exp.AllInnerExpressions)
                        yield return expAllInnerExpression;
                }
            }
        }

        public abstract double Value { get; }

        public IReadOnlyList<IMathExpression> InnerExpressions { get; protected set; } = new List<IMathExpression>();

        public bool IsConstant { get; protected set; }

        public bool IsValuable { get; protected set; } = true;

        #region Operators

        public static implicit operator MathExpression(double value) => new ValueMathExpression(value);

        public static implicit operator MathExpression(string name) => new UnknownMathExpression(name);

        public static NegMathExpression operator -(MathExpression expr) => new NegMathExpression(expr);

        public static AddMathExpression operator +(MathExpression expr1, MathExpression expr2) => new AddMathExpression(expr1, expr2);

        public static SubMathExpression operator -(MathExpression expr1, MathExpression expr2) => new SubMathExpression(expr1, expr2);

        public static MultiplicationMathExpression operator *(MathExpression expr1, MathExpression expr2) => new MultiplicationMathExpression(expr1, expr2);

        public static DivMathExpression operator /(MathExpression expr1, MathExpression expr2) => new DivMathExpression(expr1, expr2);

        public static ExpMathExpression operator ^(MathExpression expr1, MathExpression expr2) => new ExpMathExpression(expr1, expr2);

        #endregion

        public double AssignUnknowns(List<(string name, double value)> unknowns)
        {
            if (AllInnerExpressions.All(a => a.IsValuable))
                return Value;
            foreach (var mathExpression in AllInnerExpressions.Where(b => !b.IsValuable))
            {
                var expression = (UnknownMathExpression) mathExpression;
                foreach (var unknown in unknowns)
                {
                    if (expression.Name == unknown.name)
                        expression.SetUnknownValue(unknown.value);
                }
            }

            return Value;
        }

        public double AssignUnknown(string name, double value)
        {
            if (AllInnerExpressions.All(a => a.IsValuable))
                return Value;
            foreach (var mathExpression in AllInnerExpressions.Where(b => !b.IsValuable))
            {
                var unknown = (UnknownMathExpression) mathExpression;
                if (unknown.Name == name)
                    unknown.SetUnknownValue(value);
            }

            return Value;
        }

        public double SubstituteUnknowns(List<(string name, double value)> unknowns)
        {
            if (AllInnerExpressions.All(a => a.IsValuable))
                return Value;
            foreach (var mathExpression in AllInnerExpressions.Where(b => !b.IsValuable))
            {
                var expression = (UnknownMathExpression) mathExpression;
                foreach (var unknown in unknowns)
                {
                    if (expression.Name == unknown.name)
                        expression.SetUnknownValue(unknown.value);
                }
            }

            var val = Value;
            ResetUnknowns();
            return val;
        }

        public double SubstituteUnknown(string name, double value)
        {
            if (AllInnerExpressions.All(a => a.IsValuable))
                return Value;
            foreach (var mathExpression in AllInnerExpressions.Where(b => !b.IsValuable))
            {
                var expression = (UnknownMathExpression) mathExpression;
                if (expression.Name == name)
                    expression.SetUnknownValue(value);
            }

            var val = Value;
            ResetUnknowns();
            return val;
        }

        public void ResetUnknowns()
        {
            foreach (var mathExpression in AllInnerExpressions.Where(a => a is UnknownMathExpression))
            {
                var e = (UnknownMathExpression) mathExpression;
                e.ResetUnknown();
            }
        }

        public override string ToString() => $"({StringValue})";

        protected double GetValue(double value)
        {
            if (!AllInnerExpressions.All(a => a.IsValuable))
                throw new InvalidOperationException(message: "This MathExpression can't be evaluate. (Some value in expression is unknown)");
            return value;
        }
    }
}