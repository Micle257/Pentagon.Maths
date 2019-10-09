// -----------------------------------------------------------------------
//  <copyright file="Hertz.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System;
    using Converters;

    public class Hertz : IPhysicalUnit, IPhysicalConversionReferenceUnit
    {
        /// <inheritdoc />
        public MeasurementSystem System => MeasurementSystem.Unspecified;

        /// <inheritdoc />
        public string Symbol => "Hz";

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        #endregion

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