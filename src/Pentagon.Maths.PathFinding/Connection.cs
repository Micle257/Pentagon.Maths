// -----------------------------------------------------------------------
//  <copyright file="Connection.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Path
{
    using System;

    public class Connection
    {
        public double ElevationDifference => Math.Abs(NodeA.Elevation.Value - NodeB.Elevation.Value);
        public Node NodeA { get; set; }
        public Node NodeB { get; set; }
        public double Acsending { get; set; }
        public double Descending { get; set; }
    }
}