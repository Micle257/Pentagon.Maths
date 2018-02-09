// -----------------------------------------------------------------------
//  <copyright file="RacionalNumber.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Numbers
{
    public class RacionalNumber : Number
    {
        public RacionalNumber(double value)
        {
            Value = value;
        }

        public override NumberSet NumberSet => NumberSet.Racional;
        public double Value { get; }
    }
}