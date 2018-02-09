// -----------------------------------------------------------------------
//  <copyright file="UnitConverter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.Converters
{
    using Quantities;

    /// <summary> Represents a base class for all unit converters, used to converter between quantity's units. </summary>
    /// <typeparam name="TQuantity"> The type of the quantity. </typeparam>
    /// <typeparam name="TUnit"> The type of the unit. </typeparam>
    public abstract class UnitConverter<TQuantity>
        where TQuantity : IPhysicalQuantity
    {
        /// <summary> Converts the quantity into another unit. </summary>
        /// <param name="quantity"> The quantity. </param>
        /// <param name="newUnit"> The new unit. </param>
        /// <returns> A <see cref="TQuantity" />. </returns>
        public abstract TQuantity ConvertUnit(TQuantity quantity, IPhysicalUnit newUnit);

        /// <summary> Evaluates a new value from reference unit. </summary>
        /// <param name="referenceUnit"> The reference unit. </param>
        /// <param name="quantity"> The quantity. </param>
        /// <param name="newUnit"> The new unit. </param>
        /// <returns> A <see cref="double" /> of a new value. </returns>
        protected double Evaluate(IPhysicalConversionReferenceUnit referenceUnit, TQuantity quantity, IPhysicalUnit newUnit)
        {
            if (quantity.Unit.Equals(newUnit))
                return quantity.Value;

            var oldInRef = quantity.Value / referenceUnit.GetConvertRatioTo(quantity.Unit);
            return oldInRef * referenceUnit.GetConvertRatioTo(newUnit);
        }
    }
}