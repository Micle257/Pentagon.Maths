// -----------------------------------------------------------------------
//  <copyright file="Regression.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Regression
    {
        /// <summary> Fits a line to a collection of (x,y) points.TODO make good </summary>
        /// <param name="values"> Values. </param>
        /// <param name="rSquared"> The r^2 value of the line. </param>
        /// <param name="yIntercept"> The y-intercept value of the line (i.e. y = ax + b, y intercept is b). </param>
        /// <param name="slope"> The slop of the line (i.e. y = ax + b, slope is a). </param>
        public static void LinearRegression(IEnumerable<MathPoint> values,
                                            out double rSquared,
                                            out double yIntercept,
                                            out double slope)
        {
            var vals = values.ToList();
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX;
            //double ssY = 0;
            double sumCodeviates = 0;
            double sCo;
            double count = vals.Count;

            foreach (var val in vals)
            {
                var x = val.X;
                var y = val.Y;
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            //for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            //{
            //    double x = vals[ctr].X;
            //    double y = vals[ctr].Y;
            //    sumCodeviates += x * y;
            //    sumOfX += x;
            //    sumOfY += y;
            //    sumOfXSq += x * x;
            //    sumOfYSq += y * y;
            //}
            ssX = sumOfXSq - sumOfX * sumOfX / count;
            //ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            var rNumerator = count * sumCodeviates - sumOfX * sumOfY;
            var rDenom = (count * sumOfXSq - sumOfX * sumOfX)
                         * (count * sumOfYSq - sumOfY * sumOfY);
            sCo = sumCodeviates - sumOfX * sumOfY / count;

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);
            rSquared = dblR * dblR;
            yIntercept = meanY - sCo / ssX * meanX;
            slope = sCo / ssX;
        }
    }
}