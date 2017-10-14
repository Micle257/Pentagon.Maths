// -----------------------------------------------------------------------
//  <copyright file="MetreLengthUnit.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class MetreLengthUnit : ILengthUnit
    {
        /// <inheritdoc />
        public string Symbol => "m";

        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Metric;

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return true;
        }
    }
}