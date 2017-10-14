// -----------------------------------------------------------------------
//  <copyright file="FrequencyUnitConverter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.Converters
{
    using Quantities;

    /// <summary> Provides conversion logic for <see cref="Frequency" /> unit conversion. </summary>
    public class FrequencyUnitConverter : UnitConverter<Frequency>
    {
        /// <inheritdoc />
        public override Frequency ConvertUnit(Frequency quantity, IPhysicalUnit newUnit)
        {
            var value = Evaluate(new Hertz(), quantity, newUnit);
            return new Frequency(value, newUnit);
        }
    }
}