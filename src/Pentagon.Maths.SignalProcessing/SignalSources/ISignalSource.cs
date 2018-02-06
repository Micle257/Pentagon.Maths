// -----------------------------------------------------------------------
//  <copyright file="ISignalSource.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Maths.SignalProcessing.SignalSources
{
    using Quantities;

    public interface ISignalSource
    {
        Frequency SamplingFrequency { get; }

        double GetValueAt(int sample);
    }
}