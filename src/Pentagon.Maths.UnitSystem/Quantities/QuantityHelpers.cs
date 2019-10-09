// -----------------------------------------------------------------------
//  <copyright file="QuantityHelpers.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using JetBrains.Annotations;
    using Units;
    using Units.Converters;

    /// <summary> Provides helper methods for <see cref="IPhysicalQuantity" />. </summary>
    public static class QuantityHelpers
    {
        /// <summary> Compares two <see cref="IPhysicalQuantity" /> over a reference <see cref="IPhysicalUnit" />. </summary>
        /// <typeparam name="TQuantity"> The type of the quantity. </typeparam>
        /// <typeparam name="TUnit"> The type of the unit. </typeparam>
        /// <param name="first"> The first quantity. </param>
        /// <param name="second"> The second quantity. </param>
        /// <param name="referenceUnit"> The reference unit. </param>
        /// <returns> A <see cref="int" /> indicating the relative offset value of the compared quantities. </returns>
        public static int Compare<TQuantity, TUnit>([NotNull] TQuantity first, [NotNull] TQuantity second, [NotNull] TUnit referenceUnit)
                where TQuantity : IPhysicalQuantity, IUnitConvertable<TQuantity>
                where TUnit : IPhysicalUnit
        {
            var firstAsRef = first.ConvertUnit(referenceUnit);
            var secondAsRef = second.ConvertUnit(referenceUnit);
            return firstAsRef.Value.CompareTo(secondAsRef.Value);
        }

        /// <summary> Determines whether two <see cref="IPhysicalQuantity" /> are equal. </summary>
        /// <typeparam name="TQuantity"> The type of the quantity. </typeparam>
        /// <typeparam name="TUnit"> The type of the unit. </typeparam>
        /// <param name="first"> The first quantity. </param>
        /// <param name="second"> The second quantity. </param>
        /// <param name="referenceUnit"> The reference unit. </param>
        /// <returns> <c> true </c> if the quantities are equal; otherwise, <c> false </c>. </returns>
        public static bool IsEqual<TQuantity, TUnit>([NotNull] TQuantity first, [NotNull] TQuantity second, [NotNull] TUnit referenceUnit)
                where TQuantity : IPhysicalQuantity, IUnitConvertable<TQuantity>
                where TUnit : IPhysicalUnit
        {
            var firstAsRef = first.ConvertUnit(referenceUnit);
            var secondAsRef = second.ConvertUnit(referenceUnit);
            return firstAsRef.Value.Equals(secondAsRef.Value);
        }
    }
}