// -----------------------------------------------------------------------
//  <copyright file="ISamplingSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using Quantities;

    public interface ISamplingSource<out TValue>
    {
        /// <summary> Gets the frequency. </summary>
        /// <value> The <see cref="Frequency" />. </value>
        Frequency Frequency { get; }

        /// <summary> Perform looping operation on given selector. </summary>
        /// <param name="selector"> The selector. </param>
        /// <param name="samplesCount"> The samples count. </param>
        /// <param name="offsetSamples"> The offset number of samples. </param>
        /// <returns> An array of the <see cref="double" />. </returns>
        TValue[] Loop(SampleLoopCallback selector, int samplesCount, int offsetSamples = 0);

        /// <summary> Perform looping operation on given selector. </summary>
        /// <param name="selector"> The selector. </param>
        /// <param name="time"> The duration. </param>
        /// <param name="timeOffset"> The time offset. </param>
        /// <returns> An array of the <see cref="double" />. </returns>
        TValue[] Loop(SampleLoopCallback selector, TimeSpan time, TimeSpan timeOffset = default);
    }
}