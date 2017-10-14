// -----------------------------------------------------------------------
//  <copyright file="KiloMetricPrefix.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.MetricPrefixes
{
    public class KiloMetricPrefix : IMetricPrefix
    {
        /// <inheritdoc />
        public string Name => "kilo";

        /// <inheritdoc />
        public string Symbol => "k";

        /// <inheritdoc />
        public int TenthExponent => 3;

        /// <inheritdoc />
        public string WordShortScale => "thousand";

        /// <inheritdoc />
        public string WorkLongScale => WordShortScale;
    }
}