// -----------------------------------------------------------------------
//  <copyright file="Radian.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class Radian : IPlaneAngleUnit
    {
        /// <inheritdoc />
        public string Symbol => "rad";

        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit obj) => this.IsEqual(obj);
    }
}