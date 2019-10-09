// -----------------------------------------------------------------------
//  <copyright file="Kilogram.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    public class Kilogram : IPhysicalUnit
    {
        /// <inheritdoc />
        public string Symbol => "kg";

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        #endregion
    }
}