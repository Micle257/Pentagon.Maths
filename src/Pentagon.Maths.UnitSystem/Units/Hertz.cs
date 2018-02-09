// -----------------------------------------------------------------------
//  <copyright file="Hertz.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System;
    using Converters;

    public class Hertz : IPhysicalUnit, IPhysicalConversionReferenceUnit
    {
        /// <inheritdoc />
        public string Symbol => "Hz";

        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        /// <inheritdoc />
        public double GetConvertRatioTo(IPhysicalUnit unit)
        {
            //if (!(unit is IPhysicalUnit specUnit))
            //    throw new ArgumentException($"The convertion to type {unit} is not possible.");
            switch (unit)
            {
                case AngularSpeed _:
                    return 2 * Math.PI;
                default:
                    return 1;
            }
        }
    }
}