// -----------------------------------------------------------------------
//  <copyright file="Density.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    using System;
    using Helpers;
    using Units;

    public struct Density : IPhysicalQuantity, IRangeable<double>
    {
        public Density(double value) : this()
        {
            if (!Range.InRange(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;

            Unit = new ComposeUnit(new[] {new Kilogram()}, new[] {new MeterCubed()});
        }

        /// <inheritdoc />
        public IPhysicalUnit Unit { get; }

        /// <inheritdoc />
        public IRange<double> Range => new MathInterval(0, double.PositiveInfinity);

        /// <inheritdoc />
        public double Value { get; }
    }
}