// -----------------------------------------------------------------------
//  <copyright file="SamplingSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using Quantities;

    /// <summary> Represents a sampling source used to sample data. </summary>
    public struct SamplingSource : ISamplingSource<double>
    {
        /// <summary> Initializes a new instance of the <see cref="SamplingSource" /> struct. </summary>
        /// <param name="sampling"> The sampling. </param>
        /// <param name="count"> The count. </param>
        public SamplingSource(Frequency sampling)
        {
            Frequency = sampling;
        }

        /// <inheritdoc />
        public Frequency Frequency { get; }

        /// <inheritdoc />
        public double[] Loop(SampleLoopCallback selector, int samplesCount, int offset = 0)
        {
            var dt = Frequency.Period;
            var t = 0d;
            var samples = new double[samplesCount + offset];
            for (var i = 0; i < samplesCount; i++, t += dt)
                samples[i + offset] = selector(i, t);
            return samples;
        }

        /// <inheritdoc />
        public double[] Loop(SampleLoopCallback selector, TimeSpan time, TimeSpan timeOffset = default) =>
                Loop(selector, (int) (time.TotalSeconds * Frequency.Value), (int) (timeOffset.TotalSeconds * Frequency.Value));

        /// <summary> Gets the sample count which fit in given time span. </summary>
        /// <param name="span"> The span. </param>
        /// <returns> A <see cref="Int32" />. </returns>
        public int GetSampleCount(TimeSpan span) => (int) (span.TotalSeconds * Frequency.Value);

        /// <summary> Computes offset of a sample index. </summary>
        /// <param name="off"> The offset time. </param>
        /// <param name="currentSample"> The current sample index. </param>
        /// <returns> A <see cref="Int32" />. </returns>
        public int SampleOffset(TimeSpan off, int currentSample)
        {
            var count = GetSampleCount(off);
            return currentSample - count;
        }
    }
}