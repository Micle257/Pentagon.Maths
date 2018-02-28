// -----------------------------------------------------------------------
//  <copyright file="Hertz.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Converters;

    public class ComposeUnit : IPhysicalUnit
    {
        public IList<IPhysicalUnit> Numerator { get; }

        public IList<IPhysicalUnit> Denumerator { get; }

        public ComposeUnit(IEnumerable<IPhysicalUnit> numerator, IEnumerable<IPhysicalUnit> denumerator)
        {
            Numerator = numerator as IList<IPhysicalUnit> ?? numerator.ToList();
            Denumerator = denumerator as IList < IPhysicalUnit > ?? denumerator.ToList();
        }

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other)
        {
            if (!(other is ComposeUnit cu))
                return false;

            if (Numerator.Count != cu.Numerator.Count || Denumerator.Count != cu.Denumerator.Count)
                return false;

            for (var i = 0; i < Numerator.Count; i++)
            {
                if (!Numerator[i].IsEqual(cu.Numerator[i]))
                    return false;
            }

            for (var i = 0; i < Denumerator.Count; i++)
            {
                if (!Denumerator[i].IsEqual(cu.Denumerator[i]))
                    return false;
            }

            return true;
        }

        /// <inheritdoc />
        public string Symbol { get; }
    }

    public class Kilogram : IPhysicalUnit
    {
        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        /// <inheritdoc />
        public string Symbol => "kg";
    }

    public class MeterCubed : IPhysicalUnit
    {
        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other) => this.IsEqual(other);

        /// <inheritdoc />
        public string Symbol => "m^3";
    }

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