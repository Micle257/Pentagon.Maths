// -----------------------------------------------------------------------
//  <copyright file="Bisection.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.RootFinding
{
    using System;

    public class Bisection
    {
        public static bool TryFindRoot(Func<double, double> f, double lowerBound, double upperBound, double accuracy, int maxIterations, out double root)
        {
            if (upperBound < lowerBound)
            {
                var t = upperBound;
                upperBound = lowerBound;
                lowerBound = t;
            }

            double fmin = f(lowerBound);
            if (Math.Sign(fmin) == 0)
            {
                root = lowerBound;
                return true;
            }

            double fmax = f(upperBound);
            if (Math.Sign(fmax) == 0)
            {
                root = upperBound;
                return true;
            }

            root = 0.5 * (lowerBound + upperBound);

            // bad bracketing?
            if (Math.Sign(fmin) == Math.Sign(fmax))
                return false;

            for (int i = 0; i <= maxIterations; i++)
            {
                double froot = f(root);

                if (upperBound - lowerBound <= 2 * accuracy && Math.Abs(froot) <= accuracy)
                    return true;

                if (lowerBound == root || upperBound == root)
                {
                    // accuracy not sufficient, but cannot be improved further
                    return false;
                }

                if (Math.Sign(froot) == Math.Sign(fmin))
                {
                    lowerBound = root;
                    fmin = froot;
                }
                else if (Math.Sign(froot) == Math.Sign(fmax))
                {
                    upperBound = root;
                    fmax = froot;
                }
                else // Math.Sign(froot) == 0
                    return true;

                root = 0.5 * (lowerBound + upperBound);
            }

            return false;
        }
    }
}