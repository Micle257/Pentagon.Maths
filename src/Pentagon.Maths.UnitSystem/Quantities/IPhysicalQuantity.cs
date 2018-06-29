// -----------------------------------------------------------------------
//  <copyright file="IPhysicalQuantity.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using Units;

    /// <summary> Represents a physical property that can be quantified by measurement. </summary>
    public interface IPhysicalQuantity
    {
        /// <summary> Gets the unit. </summary>
        /// <value> The <see cref="IPhysicalUnit" />. </value>
        IPhysicalUnit Unit { get; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        double Value { get; }
    }
}