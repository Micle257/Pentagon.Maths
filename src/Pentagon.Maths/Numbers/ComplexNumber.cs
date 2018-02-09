// -----------------------------------------------------------------------
//  <copyright file="ComplexNumber.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Numbers
{
    using System.Numerics;

    public class ComplexNumber : Number
    {
        public ComplexNumber(Complex value)
        {
            Value = value;
        }

        public override NumberSet NumberSet => NumberSet.Complex;

        public Complex Value { get; }
    }
}