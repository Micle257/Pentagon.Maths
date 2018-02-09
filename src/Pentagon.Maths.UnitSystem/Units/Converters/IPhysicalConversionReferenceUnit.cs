// -----------------------------------------------------------------------
//  <copyright file="IPhysicalConversionReferenceUnit.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.Converters
{
    /// <summary> Represents an unit which is used as reference to convertions between similar quantities. </summary>
    public interface IPhysicalConversionReferenceUnit : IPhysicalUnit
    {
        /// <summary> Gets the convert compute ratio to another non-reference unit. </summary>
        /// <param name="unit"> The unit. </param>
        /// <returns> A <see cref="double" />. </returns>
        double GetConvertRatioTo(IPhysicalUnit unit);
    }
}