// -----------------------------------------------------------------------
//  <copyright file="IMathExpression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expression
{
    using System.Collections.Generic;

    public interface IMathExpression
    {
        IReadOnlyList<IMathExpression> InnerExpressions { get; }
        IEnumerable<IMathExpression> AllInnerExpressions { get; }
        bool IsConstant { get; }
        bool IsValuable { get; }
        double Value { get; }
        double AssignUnknowns(List<(string name, double value)> unknowns);
        double AssignUnknown(string name, double value);
        double SubstituteUnknowns(List<(string name, double value)> unknowns);
        double SubstituteUnknown(string name, double value);
        void ResetUnknowns();
    }
}