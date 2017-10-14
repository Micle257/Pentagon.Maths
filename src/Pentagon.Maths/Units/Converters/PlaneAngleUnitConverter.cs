// -----------------------------------------------------------------------
//  <copyright file="PlaneAngleUnitConverter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.Converters
{
    using Quantities;

    /// <summary> Provides conversion logic for <see cref="PlaneAngle" /> unit conversion. </summary>
    public class PlaneAngleUnitConverter : UnitConverter<PlaneAngle>
    {
        /// <inheritdoc />
        public override PlaneAngle ConvertUnit(PlaneAngle quantity, IPhysicalUnit newUnit)
        {
            var value = Evaluate(new Degree(), quantity, newUnit);
            return new PlaneAngle(value, newUnit);
        }
    }
}