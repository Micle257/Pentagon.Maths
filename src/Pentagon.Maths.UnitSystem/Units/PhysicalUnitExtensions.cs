// -----------------------------------------------------------------------
//  <copyright file="PhysicalUnitExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    /// <summary> Provides extensions methods for <see cref="IPhysicalUnit" />. </summary>
    public static class PhysicalUnitExtensions
    {
        /// <summary> Determines if two <see cref="IPhysicalUnit" /> are same or of the same type. </summary>
        /// <param name="reference"> The reference. </param>
        /// <param name="other"> The other. </param>
        /// <returns> <c> true </c> if units are equal; otherwise, <c> false </c>. </returns>
        public static bool IsEqual(this IPhysicalUnit reference, IPhysicalUnit other)
        {
            if (ReferenceEquals(null, other) || ReferenceEquals(null, reference))
                return false;
            if (ReferenceEquals(reference, other))
                return true;
            return other.GetType() == reference.GetType();
        }
    }
}