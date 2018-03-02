// -----------------------------------------------------------------------
//  <copyright file="ISystemManagerConstrain.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System.Collections.Generic;
    using Abstractions;

    public interface ISystemManagerConstrain
    {
        INode Node { get; }
        bool Run(double outputValue, IEnumerable<double> inputValues);
    }
}