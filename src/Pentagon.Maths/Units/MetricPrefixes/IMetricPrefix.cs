// -----------------------------------------------------------------------
//  <copyright file="IMetricPrefix.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units.MetricPrefixes
{
    public interface IMetricPrefix
    {
        string Name { get; }
        string Symbol { get; }
        int TenthExponent { get; }
        string WordShortScale { get; }
        string WorkLongScale { get; }
    }
}