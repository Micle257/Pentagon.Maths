// -----------------------------------------------------------------------
//  <copyright file="IPhysicalQuantity.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using Units;

    /// <summary> Represents a physical property that can be quantified by measurement. </summary>
    public interface IPhysicalQuantity : IValuable<double>
    {
        /// <summary> Gets the unit. </summary>
        /// <value> The <see cref="IPhysicalUnit" />. </value>
        IPhysicalUnit Unit { get; }
    }
}