// -----------------------------------------------------------------------
//  <copyright file="DifferenceEquationBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class DifferenceEquationBuilder
    {
        public Expression<Func<RelativeSignal, RelativeSignal, double>> GetDifferenceEqutionExpression(TransferFunction function)
        {
            var inputs = function.Numerator.Coefficients;
            var outputs = function.Denumerator.Coefficients;

            var inExp = new BinaryExpression[inputs.Count];
            var outExp = new BinaryExpression[inputs.Count - 1];

            var xParam = Expression.Parameter(typeof(RelativeSignal), name: "x");
            var yParam = Expression.Parameter(typeof(RelativeSignal), name: "y");

            for (var i = 0; i < inputs.Count; i++)
            {
                var con = Expression.Constant(inputs[i]);
                var arg = -i;

                var indexer = typeof(RelativeSignal).GetProperties().Where(a => a.GetIndexParameters().Length > 0).FirstOrDefault();

                var call = Expression.Call(xParam, indexer.GetGetMethod(), Expression.Constant(arg));

                var member = Expression.Multiply(con, call);

                inExp[i] = member;
            }

            for (var i = 0; i < inputs.Count - 1; i++)
            {
                var con = Expression.Constant(-outputs[i + 1]);
                var arg = -i;

                var indexer = typeof(RelativeSignal).GetProperties().Where(a => a.GetIndexParameters().Length > 0).FirstOrDefault();

                var call = Expression.Call(yParam, indexer.GetGetMethod(), Expression.Constant(arg));

                var member = Expression.Multiply(con, call);

                outExp[i] = member;
            }

            var inAdd = inExp.Aggregate((a, b) => Expression.Add(a, b));
            var outAdd = outExp.Aggregate((a, b) => Expression.Add(a, b));

            var finalAdd = Expression.Add(inAdd, outAdd);

            var lambda = Expression.Lambda(finalAdd, xParam, yParam);

            return (Expression<Func<RelativeSignal, RelativeSignal, double>>) lambda;
        }

        public DifferenceEquation Build(TransferFunction function) => new DifferenceEquation(function.Numerator.Coefficients, function.Denumerator.Coefficients);
    }
}