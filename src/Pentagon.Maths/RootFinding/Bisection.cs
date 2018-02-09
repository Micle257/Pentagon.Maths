// -----------------------------------------------------------------------
//  <copyright file="Bisection.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.RootFinding
{
    using System;

    //public static class Broyden
    //{
    //    /// <summary>Find a solution of the equation f(x)=0.</summary>
    //    /// <param name="f">The function to find roots from.</param>
    //    /// <param name="initialGuess">Initial guess of the root.</param>
    //    /// <param name="accuracy">Desired accuracy. The root will be refined until the accuracy or the maximum number of iterations is reached. Default 1e-8.</param>
    //    /// <param name="maxIterations">Maximum number of iterations. Default 100.</param>
    //    /// <param name="jacobianStepSize">Relative step size for calculating the Jacobian matrix at first step. Default 1.0e-4</param>
    //    /// <returns>Returns the root with the specified accuracy.</returns>
    //    public static double[] FindRoot(Func<double[], double[]> f, double[] initialGuess, double accuracy = 1e-8, int maxIterations = 100, double jacobianStepSize = 1.0e-4)
    //    {
    //        double[] root;
    //        if (TryFindRootWithJacobianStep(f, initialGuess, accuracy, maxIterations, jacobianStepSize, out root))
    //        {
    //            return root;
    //        }

    //        throw new Exception();
    //    }

    //    /// <summary>Find a solution of the equation f(x)=0.</summary>
    //    /// <param name="f">The function to find roots from.</param>
    //    /// <param name="initialGuess">Initial guess of the root.</param>
    //    /// <param name="accuracy">Desired accuracy. The root will be refined until the accuracy or the maximum number of iterations is reached.</param>
    //    /// <param name="maxIterations">Maximum number of iterations. Usually 100.</param>
    //    /// <param name="jacobianStepSize">Relative step size for calculating the Jacobian matrix at first step.</param>
    //    /// <param name="root">The root that was found, if any. Undefined if the function returns false.</param>
    //    /// <returns>True if a root with the specified accuracy was found, else false.</returns>
    //    public static bool TryFindRootWithJacobianStep(Func<double[], double[]> f, double[] initialGuess, double accuracy, int maxIterations, double jacobianStepSize, out double[] root)
    //    {
    //        var x = new DenseVector(initialGuess);

    //        double[] y0 = f(initialGuess);
    //        var y = new DenseVector(y0);
    //        double g = y.L2Norm();

    //        Matrix<double> B = CalculateApproximateJacobian(f, initialGuess, y0, jacobianStepSize);

    //        for (int i = 0; i <= maxIterations; i++)
    //        {
    //            var dx = (DenseVector)(-B.LU().Solve(y));
    //            var xnew = x + dx;
    //            var ynew = new DenseVector(f(xnew.Values));
    //            double gnew = ynew.L2Norm();

    //            if (gnew > g)
    //            {
    //                double g2 = g * g;
    //                double scale = g2 / (g2 + gnew * gnew);
    //                if (scale == 0.0)
    //                {
    //                    scale = 1.0e-4;
    //                }

    //                dx = scale * dx;
    //                xnew = x + dx;
    //                ynew = new DenseVector(f(xnew.Values));
    //                gnew = ynew.L2Norm();
    //            }

    //            if (gnew < accuracy)
    //            {
    //                root = xnew.Values;
    //                return true;
    //            }

    //            // update Jacobian B
    //            DenseVector dF = ynew - y;
    //            Matrix<double> dB = (dF - B.Multiply(dx)).ToColumnMatrix() * dx.Multiply(1.0 / Math.Pow(dx.L2Norm(), 2)).ToRowMatrix();
    //            B = B + dB;

    //            x = xnew;
    //            y = ynew;
    //            g = gnew;
    //        }

    //        root = null;
    //        return false;
    //    }
    //    /// <summary>Find a solution of the equation f(x)=0.</summary>
    //    /// <param name="f">The function to find roots from.</param>
    //    /// <param name="initialGuess">Initial guess of the root.</param>
    //    /// <param name="accuracy">Desired accuracy. The root will be refined until the accuracy or the maximum number of iterations is reached.</param>
    //    /// <param name="maxIterations">Maximum number of iterations. Usually 100.</param>
    //    /// <param name="root">The root that was found, if any. Undefined if the function returns false.</param>
    //    /// <returns>True if a root with the specified accuracy was found, else false.</returns>
    //    public static bool TryFindRoot(Func<double[], double[]> f, double[] initialGuess, double accuracy, int maxIterations, out double[] root)
    //    {
    //        return TryFindRootWithJacobianStep(f, initialGuess, accuracy, maxIterations, 1.0e-4, out root);
    //    }

    //    /// <summary>
    //    /// Helper method to calculate an approximation of the Jacobian.
    //    /// </summary>
    //    /// <param name="f">The function.</param>
    //    /// <param name="x0">The argument (initial guess).</param>
    //    /// <param name="y0">The result (of initial guess).</param>
    //    /// <param name="jacobianStepSize">Relative step size for calculating the Jacobian.</param>
    //    static Matrix<double> CalculateApproximateJacobian(Func<double[], double[]> f, double[] x0, double[] y0, double jacobianStepSize)
    //    {
    //        int dim = x0.Length;
    //        var B = new DenseMatrix(dim);

    //        var x = new double[dim];
    //        Array.Copy(x0, 0, x, 0, dim);

    //        for (int j = 0; j < dim; j++)
    //        {
    //            double h = (1.0 + Math.Abs(x0[j])) * jacobianStepSize;

    //            var xj = x[j];
    //            x[j] = xj + h;
    //            double[] y = f(x);
    //            x[j] = xj;

    //            for (int i = 0; i < dim; i++)
    //            {
    //                B.At(i, j, (y[i] - y0[i]) / h);
    //            }
    //        }

    //        return B;
    //    }
    //}

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