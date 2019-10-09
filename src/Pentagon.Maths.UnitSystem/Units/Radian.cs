// -----------------------------------------------------------------------
//  <copyright file="Radian.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class Radian : IPlaneAngleUnit
    {
        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public string Symbol => "rad";

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit obj) => this.IsEqual(obj);

        #endregion
    }
}