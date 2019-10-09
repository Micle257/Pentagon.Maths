// -----------------------------------------------------------------------
//  <copyright file="GeographicLocation.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.PathFinding
{
    public class GeographicLocation
    {
        public GpsCoordinate Coordinate { get; set; }
        public Elevation Elevation { get; set; }
    }
}