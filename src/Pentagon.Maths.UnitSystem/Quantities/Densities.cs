// -----------------------------------------------------------------------
//  <copyright file="Densities.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Quantities
{
    public static class Densities
    {
        public static Density Steel => new Density(7860);

        public static Density Copper => new Density(8960);
    }
}