﻿// -----------------------------------------------------------------------
//  <copyright file="Unitless.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    /// <summary> Represents an unitless physical unit. </summary>
    public class Unitless : IPhysicalUnit
    {
        /// <inheritdoc />
        public string Symbol => string.Empty;

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        #endregion
    }
}