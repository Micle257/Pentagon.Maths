// -----------------------------------------------------------------------
//  <copyright file="MeterCubed.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class MeterCubed : IPhysicalUnit
    {
        /// <inheritdoc />
        public string Symbol => "m^3";

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        #endregion
    }
}