// -----------------------------------------------------------------------
//  <copyright file="StringExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression.String
{
    using System.Collections.Generic;
    using System.Linq;

    public class StringExpression //: MathExpression
    {
        readonly List<IMathExpression> _exprs = new List<IMathExpression>();
        IMathExpression _mathExpression;

        public StringExpression(string v)
        {
            StringValue = v;
            var inExpression = false;
            var ws = new List<ExpressionWriter>();
            ExpressionWriter writer = null;
            var brace = 0;
            double test;
            if (double.TryParse(v, out test))
            {
                IsValue = true;
                _mathExpression = new ValueMathExpression(test);
            }
            foreach (var ch in v)
            {
                if (!inExpression)
                {
                    if (ch == '(')
                    {
                        inExpression = true;
                        writer = new ExpressionWriter();
                        ws.Add(writer);
                    }
                    else
                    {
                        Operation += ch.ToString();
                        //curow.Oper = ch;
                    }
                }
                else
                {
                    if (ch == '(')
                        brace++;
                    else
                    {
                        if (ch == ')')
                        {
                            if (brace > 0)
                                brace--;
                            else
                                inExpression = false;
                        }
                    }
                    if (inExpression)
                        writer.AddChar(ch);
                }
            }
            foreach (var item in ws)
            {
                var stringExpression = new StringExpression(item.Expression);
                InnerStringExpressions.Add(stringExpression);
            }
        }

        public double Value => _mathExpression.Value;

        public string StringValue { get; }

        public List<StringExpression> InnerStringExpressions { get; } = new List<StringExpression>();

        public bool IsValue { get; }

        public string Operation { get; }

        public IReadOnlyList<IMathExpression> InnerExpressions => _exprs;

        public bool IsComputed { get; private set; }

        public IMathExpression Compute()
        {
            foreach (var ex in InnerStringExpressions)
            {
                var sad = ex.Compute();
                _exprs.Add(sad);
            }
            if (IsValue)
                return new ValueMathExpression(Value);
            if (Operation == "+")
                _mathExpression = new AddMathExpression(InnerExpressions.ToArray());
            if (Operation == "*")
                _mathExpression = new MultiplicationMathExpression(InnerExpressions.ToArray());
            if (Operation == "log")
                _mathExpression = new LogMathExpression(10, InnerExpressions.FirstOrDefault());
            if (Operation == "ln")
                _mathExpression = new LogMathExpression(2.71, InnerExpressions.FirstOrDefault());
            if (Operation == "sin")
                _mathExpression = new SinMathExpression(InnerExpressions.FirstOrDefault());
            if (Operation == "cos")
                _mathExpression = new CosMathExpression(InnerExpressions.FirstOrDefault());
            if (Operation == "-")
                _mathExpression = new NegMathExpression(InnerExpressions.FirstOrDefault());
            IsComputed = true;
            return _mathExpression;
        }
    }
}