// -----------------------------------------------------------------------
//  <copyright file="IUnitConvertable.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.Converters
{
    using JetBrains.Annotations;
    using Quantities;

    /// <summary> Represents a quantity that can be converter into another unit. </summary>
    /// <typeparam name="TQuantity"> The type of the quantity. </typeparam>
    /// <typeparam name="TUnit"> The type of the unit. </typeparam>
    public interface IUnitConvertable<out TQuantity>
        where TQuantity : IPhysicalQuantity
    {
        /// <summary>
        ///     Gets the reference unit.
        /// </summary>
        /// <value>
        ///     The <see cref="TUnit" />.
        /// </value>
        [NotNull]
        IPhysicalConversionReferenceUnit ReferenceUnit { get; }

        /// <summary> Converts this quantity into another unit. </summary>
        /// <param name="newUnit"> The new unit. </param>
        /// <returns> A <see cref="TQuantity" />. </returns>
        TQuantity ConvertUnit([NotNull] IPhysicalUnit newUnit);
    }
}