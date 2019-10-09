// -----------------------------------------------------------------------
//  <copyright file="Length.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using System;
    using Helpers;
    using Units;

    public struct Length : IPhysicalQuantity, IRangeable<double>
    {
        public Length(double value, ILengthUnit unit = null) : this()
        {
            if (!Range.InRange(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;

            Unit = unit ?? new MetreLengthUnit();
        }

        /// <inheritdoc />
        public IPhysicalUnit Unit { get; }

        /// <inheritdoc />
        public IRange<double> Range => new MathInterval(0, double.PositiveInfinity);

        /// <inheritdoc />
        public double Value { get; }
    }
}