// -----------------------------------------------------------------------
//  <copyright file="ParameterFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Functions
{
    using Expression;

    class ParameterFunction
    {
        public const string ParameterName = "t";

        public ParameterFunction(IMathExpression xFunction, IMathExpression yFunction)
        {
            XFunction = xFunction;
            YFunction = yFunction;
        }

        public IMathExpression XFunction { get; }
        public IMathExpression YFunction { get; }
    }
}