// -----------------------------------------------------------------------
//  <copyright file="GeographicConnection.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.PathFinding
{
    using System;

    public class GeographicConnection
    {
        public double ElevationDifference => Math.Abs(Location1.Elevation.Value - Location2.Elevation.Value);

        public GeographicLocation Location1 { get; set; }

        public GeographicLocation Location2 { get; set; }

        public double Acsending { get; set; }

        public double Descending { get; set; }
    }
}