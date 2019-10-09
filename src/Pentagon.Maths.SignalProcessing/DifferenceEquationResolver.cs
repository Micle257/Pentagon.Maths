// -----------------------------------------------------------------------
//  <copyright file="DifferenceEquationResolver.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Pentagon.Extensions;

    public class DifferenceEquationResolver
    {
        public enum MemberAdditionMethod
        {
            Add,
            Subtract
        }

        enum MemberType
        {
            Unspecified,
            ConstantTimesValue,
            Value
        }

        public IList<(ValueDirection Direction, double Coefficient, int Delay)> Resolve(IList<(Expression, MemberAdditionMethod)> mems, IDictionary<string, ValueDirection> parMap)
        {
            var result = new List<(ValueDirection, double, int)>();

            foreach (var m in mems.Where(a => IsMemberExpression(a.Item1)))
            {
                var coeff = GetCoefficient(m.Item1, m.Item2);
                var valueDirection = GetDirection(m.Item1, parMap);
                var delay = GetDelay(m.Item1);

                result.Add((valueDirection, coeff, delay));
            }

            return result;
        }

        public IList<(Expression, MemberAdditionMethod)> ResolveMembers(Expression expression)
        {
            var exprs = new List<(Expression, MemberAdditionMethod)>();

            if (IsAddOrSubtractExpression(expression))
            {
                var binExp = (BinaryExpression) expression;

                var left = binExp.Left;
                var right = binExp.Right;

                if (IsMemberExpression(left))
                    exprs.Add((left, MemberAdditionMethod.Add));
                else
                    exprs.AddRange(ResolveMembers(left));

                if (IsMemberExpression(right))
                {
                    switch (expression.NodeType)
                    {
                        case ExpressionType.Add:
                        case ExpressionType.AddChecked:
                            exprs.Add((right, MemberAdditionMethod.Add));
                            break;

                        case ExpressionType.Subtract:
                        case ExpressionType.SubtractChecked:
                            exprs.Add((right, MemberAdditionMethod.Subtract));
                            break;
                    }
                }
                else
                    exprs.AddRange(ResolveMembers(right));
            }

            return exprs;
        }

        public SystemTuple GetDefinition(IList<(ValueDirection Direction, double Coefficient, int Delay)> members)
        {
            var inputs = new Dictionary<int, double>();
            var outputs = new Dictionary<int, double>();

            foreach (var m in members)
            {
                if (m.Direction == ValueDirection.Input)
                {
                    if (inputs.ContainsKey(m.Delay))
                        throw new ArgumentException(message: "The delay members must be unique.");

                    inputs.Add(Math.Abs(m.Delay), m.Coefficient);
                }
                else if (m.Direction == ValueDirection.Output)
                {
                    if (outputs.ContainsKey(m.Delay))
                        throw new ArgumentException(message: "The delay members must be unique.");

                    outputs.Add(Math.Abs(m.Delay), m.Coefficient);
                }
            }

            var resultLength = inputs.Concat(outputs).Select(a => a.Key).Max() + 1;

            var num = new double[resultLength]; // in
            var den = new double[resultLength]; // out
            var b0 = 1d;

            for (var i = 0; i < resultLength; i++)
            {
                if (outputs.TryGetValue(i, out var value))
                {
                    if (i == 0)
                    {
                        b0 = 1 - value;
                        den[i] = 1;
                    }
                    else
                        den[i] = -value / b0;
                }

                if (inputs.TryGetValue(i, out value))
                    num[i] = value / b0;
            }

            return new SystemTuple(num, den);
        }

        public SystemTuple GetCoefficients(Expression<Func<RelativeSignal, RelativeSignal, double>> expression)
        {
            var parameterNames = expression.Parameters.Select(a => a.Name).ToArray();

            if (parameterNames.Length != 2)
                throw new ArgumentException();

            var parameterMap = new Dictionary<string, ValueDirection>
                               {
                                       {parameterNames[0], ValueDirection.Input},
                                       {parameterNames[1], ValueDirection.Output}
                               };

            var members = ResolveMembers(expression.Body);
            var resolve = Resolve(members, parameterMap);

            return GetDefinition(resolve);
        }

        ValueDirection GetDirection(Expression expression, IDictionary<string, ValueDirection> parMap)
        {
            var type = GetMemberType(expression);

            switch (type)
            {
                case MemberType.ConstantTimesValue:
                {
                    var binaryExpression = (BinaryExpression) expression;
                    Expression indexObject;
                    if (binaryExpression.Left.NodeType == ExpressionType.Call)
                        indexObject = ((MethodCallExpression) binaryExpression.Left).Object;
                    else if (binaryExpression.Right.NodeType == ExpressionType.Call)
                        indexObject = ((MethodCallExpression) binaryExpression.Right).Object;
                    else
                        throw new ArgumentException(message: "The given binary expression has no index expression.");

                    if (indexObject.NodeType != ExpressionType.Parameter)
                        throw new ArgumentException(message: "The index object must be a parameter.");

                    var parameter = (ParameterExpression) indexObject;

                    if (!parMap.TryGetValue(parameter.Name, out var direction))
                        throw new ArgumentException(message: "The parameter was not mapped to any direction.");

                    return direction;
                }

                case MemberType.Value:
                {
                    var indexExpression = (MethodCallExpression) expression;
                    var indexObject = indexExpression.Object;

                    if (indexObject.NodeType != ExpressionType.Parameter)
                        throw new ArgumentException(message: "The index object must be a parameter.");

                    var parameter = (ParameterExpression) indexObject;

                    if (!parMap.TryGetValue(parameter.Name, out var direction))
                        throw new ArgumentException(message: "The parameter was not mapped to any direction.");

                    return direction;
                }
            }

            throw new ArgumentException();
        }

        MemberType GetMemberType(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Multiply:
                    return MemberType.ConstantTimesValue;

                case ExpressionType.Call:
                    return MemberType.Value;
            }

            return MemberType.Unspecified;
        }

        int GetDelay(Expression expression)
        {
            var type = GetMemberType(expression);

            switch (type)
            {
                case MemberType.ConstantTimesValue:
                {
                    var binaryExpression = (BinaryExpression) expression;
                    MethodCallExpression boxedCoefficient;

                    if (binaryExpression.Left.NodeType == ExpressionType.Call)
                        boxedCoefficient = (MethodCallExpression) binaryExpression.Left;
                    else if (binaryExpression.Right.NodeType == ExpressionType.Call)
                        boxedCoefficient = (MethodCallExpression) binaryExpression.Right;
                    else
                        throw new ArgumentException(message: "The given binary expression has no call expression.");

                    return GetDelayFromCallExpression(boxedCoefficient);
                }
                case MemberType.Value:
                {
                    return GetDelayFromCallExpression((MethodCallExpression) expression);
                }
            }

            throw new ArgumentException();
        }

        int GetDelayFromCallExpression(MethodCallExpression expression)
        {
            var indexParameters = expression.Arguments;

            if (indexParameters.Count != 1)
                throw new ArgumentException(message: "The index expression must have one argument.");

            var param = indexParameters[0];

            switch (param.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                {
                    var inner = (UnaryExpression) param;
                    if (inner.Operand.NodeType == ExpressionType.Constant)
                    {
                        var boxedCoefficient = ((ConstantExpression) inner.Operand).Value;

                        if (!int.TryParse(boxedCoefficient.ToString(), out var coefficient))
                            throw new ArgumentException(message: "The coefficient is not a number.");

                        if (coefficient > 0)
                            throw new ArgumentException(message: "The coefficient must be less than or equal to zero.");

                        return coefficient;
                    }

                    throw new ArgumentException(message: "The expression nested in negate expression must be a constant expression.");
                }

                case ExpressionType.Constant:
                {
                    var boxedCoefficient = ((ConstantExpression) param).Value;

                    if (!int.TryParse(boxedCoefficient.ToString(), out var coefficient))
                        throw new ArgumentException(message: "The coefficient is not a number.");

                    if (coefficient > 0)
                        throw new ArgumentException(message: "The coefficient must be less than or equal to zero.");

                    return coefficient;
                }

                default:
                    throw new ArgumentException(message: "This expression for index argument is not supported.");
            }
        }

        double GetCoefficient(Expression expression, MemberAdditionMethod additionMethod)
        {
            var type = GetMemberType(expression);

            switch (type)
            {
                case MemberType.ConstantTimesValue:
                {
                    var binaryExpression = (BinaryExpression) expression;
                    object boxedCoefficient;

                    if (binaryExpression.Left.NodeType == ExpressionType.Constant)
                        boxedCoefficient = ((ConstantExpression) binaryExpression.Left).Value;
                    else if (binaryExpression.Right.NodeType == ExpressionType.Constant)
                        boxedCoefficient = ((ConstantExpression) binaryExpression.Right).Value;
                    else if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        var memberConstantExpression = (MemberExpression) binaryExpression.Left;
                        boxedCoefficient = memberConstantExpression.GetMemberValue();
                    }
                    else if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        var memberConstantExpression = (MemberExpression) binaryExpression.Right;
                        boxedCoefficient = memberConstantExpression.GetMemberValue();
                    }
                    else
                        throw new ArgumentException(message: "The given binary expression has no constant expression.");

                    if (!double.TryParse(boxedCoefficient.ToString(), out var coefficient))
                        throw new ArgumentException(message: "The coefficient is not a decimal number.");

                    if (additionMethod == MemberAdditionMethod.Subtract)
                        return -coefficient;
                    if (additionMethod == MemberAdditionMethod.Add)
                        return coefficient;
                    throw new ArgumentException(message: "The addition method is not specified.");
                }

                case MemberType.Value:
                    if (additionMethod == MemberAdditionMethod.Subtract)
                        return -1;
                    else if (additionMethod == MemberAdditionMethod.Add)
                        return 1;
                    else
                        throw new ArgumentException(message: "The addition method is not specified.");
            }

            throw new ArgumentException();
        }

        bool IsMemberExpression(Expression e)
        {
            switch (e.NodeType)
            {
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Multiply:
                case ExpressionType.Call:
                    return true;
            }

            return false;
        }

        bool IsAddOrSubtractExpression(Expression e)
        {
            switch (e.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return true;
            }

            return false;
        }
    }
}