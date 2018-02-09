// -----------------------------------------------------------------------
//  <copyright file="Degree.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System;
    using Converters;

    public class Degree : IPlaneAngleUnit, IPhysicalConversionReferenceUnit
    {
        /// <inheritdoc />
        public string Symbol => "°";

        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit obj) => this.IsEqual(obj);

        /// <inheritdoc />
        public double GetConvertRatioTo(IPhysicalUnit unit)
        {
            if (!(unit is IPlaneAngleUnit))
                throw new ArgumentException($"The convertion to type {unit} is not possible.");
            switch (unit)
            {
                case Radian _:
                    return Math.PI / 180;
                default:
                    return 1;
            }
        }
    }
}