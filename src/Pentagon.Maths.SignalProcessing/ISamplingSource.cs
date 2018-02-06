// -----------------------------------------------------------------------
//  <copyright file="ISamplingSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using Quantities;

    public interface ISamplingSource<TValue>
    {
        Frequency Frequency { get; }
        int Count { get; }
        TValue[] Loop(SampleLoopCallback selector, int offsetSamples = 0);
    }
}