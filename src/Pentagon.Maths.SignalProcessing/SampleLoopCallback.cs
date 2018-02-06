// -----------------------------------------------------------------------
//  <copyright file="SampleLoopCallback.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    /// <summary>
    ///     Encapsulates a method for computing looping algorithm.
    /// </summary>
    /// <param name="iteration"> The iteration. </param>
    /// <param name="time"> The time. </param>
    /// <returns> A value. </returns>
    public delegate double SampleLoopCallback(int iteration, double time);
}