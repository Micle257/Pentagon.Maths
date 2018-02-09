// -----------------------------------------------------------------------
//  <copyright file="IPhysicalUnit.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System;

    /// <summary> Represents a magnitude/measurement of a quantity. </summary>
    public interface IPhysicalUnit : IEquatable<IPhysicalUnit>
    {
        /// <summary> Gets the symbol of this unit. </summary>
        /// <value> The <see cref="String" />. </value>
        string Symbol { get; }
    }
}