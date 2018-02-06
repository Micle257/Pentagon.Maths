// -----------------------------------------------------------------------
//  <copyright file="SamplingSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using Quantities;

    /// <summary>
    ///     Represents a sampling source used to sample data.
    /// </summary>
    public struct SamplingSource : ISamplingSource<double>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SamplingSource" /> struct.
        /// </summary>
        /// <param name="sampling"> The sampling. </param>
        /// <param name="count"> The count. </param>
        public SamplingSource(Frequency sampling, int count)
        {
            Frequency = sampling;
            Count = count;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SamplingSource" /> struct.
        /// </summary>
        /// <param name="frequency"> The frequency. </param>
        /// <param name="timeSpan"> The time span. </param>
        public SamplingSource(Frequency frequency, TimeSpan timeSpan) : this(frequency, (int) (timeSpan.TotalSeconds * frequency.Value)) { }

        /// <summary>
        ///     Gets the frequency.
        /// </summary>
        /// <value>
        ///     The <see cref="Frequency" />.
        /// </value>
        public Frequency Frequency { get; }

        /// <summary>
        ///     Gets the count.
        /// </summary>
        /// <value>
        ///     The <see cref="Int32" />.
        /// </value>
        public int Count { get; }

        /// <summary>
        ///     Perform looping operation on given selector.
        /// </summary>
        /// <param name="selector"> The selector. </param>
        /// <param name="offset"> The offset number of samples. </param>
        /// <returns>
        ///     An array of the <see cref="double" />.
        /// </returns>
        public double[] Loop(SampleLoopCallback selector, int offset = 0)
        {
            var dt = Frequency.Period;
            var t = 0d;
            var samples = new double[Count + offset];
            for (var i = 0; i < Count; i++, t += dt)
                samples[i + offset] = selector(i, t);
            return samples;
        }

        /// <summary>
        ///     Gets the sample count which fit in given time span.
        /// </summary>
        /// <param name="span"> The span. </param>
        /// <returns>
        ///     A <see cref="Int32" />.
        /// </returns>
        public int GetSampleCount(TimeSpan span) => (int) (span.TotalSeconds * Frequency.Value);

        /// <summary>
        ///     Computes offset of a sample index.
        /// </summary>
        /// <param name="off"> The offset time. </param>
        /// <param name="currentSample"> The current sample index. </param>
        /// <returns>
        ///     A <see cref="Int32" />.
        /// </returns>
        public int SampleOffset(TimeSpan off, int currentSample)
        {
            var count = GetSampleCount(off);
            return currentSample - count;
        }

        /// <summary>
        ///     Perform looping operation on given selector.
        /// </summary>
        /// <param name="selector"> The selector. </param>
        /// <param name="offset"> The time offset. </param>
        /// <returns> An array of the <see cref="double" />. </returns>
        public double[] Loop(SampleLoopCallback selector, TimeSpan offset)
        {
            var offSam = GetSampleCount(offset);
            return Loop(selector, offSam);
        }
    }
}