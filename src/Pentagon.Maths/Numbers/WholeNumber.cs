// -----------------------------------------------------------------------
//  <copyright file="WholeNumber.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Numbers
{
    public class WholeNumber : Number
    {
        public WholeNumber(int value)
        {
            Value = value;
        }

        public override NumberSet NumberSet => NumberSet.Whole;
        public int Value { get; }
    }
}