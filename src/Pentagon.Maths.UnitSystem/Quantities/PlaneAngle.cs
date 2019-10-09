// -----------------------------------------------------------------------
//  <copyright file="PlaneAngle.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using Units;
    using Units.Converters;

    /// <summary> Represents an angle quantity which measures the ratio of the length of a circular arc to its radus. </summary>
    public struct PlaneAngle : IPhysicalQuantity, IUnitConvertable<PlaneAngle>
    {
        /// <summary> Initializes a new instance of the <see cref="PlaneAngle" /> struct. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="unit"> The unit. </param>
        public PlaneAngle(double value, IPhysicalUnit unit)
        {
            Value = value;
            Unit = unit;
            HasValue = true;
        }

        /// <inheritdoc />
        public bool HasValue { get; }

        /// <inheritdoc />
        public IPhysicalUnit Unit { get; }

        /// <inheritdoc />
        public IPhysicalConversionReferenceUnit ReferenceUnit => new Degree();

        /// <inheritdoc />
        public double Value { get; }

        /// <inheritdoc />
        public PlaneAngle ConvertUnit(IPhysicalUnit newUnit) => new PlaneAngleUnitConverter().ConvertUnit(this, newUnit);
    }
}