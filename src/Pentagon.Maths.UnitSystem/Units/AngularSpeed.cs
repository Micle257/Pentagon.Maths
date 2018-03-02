// -----------------------------------------------------------------------
//  <copyright file="AngularSpeed.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class AngularSpeed : IPhysicalUnit
    {
        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public string Symbol => "rad/s";

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        #endregion
    }
}