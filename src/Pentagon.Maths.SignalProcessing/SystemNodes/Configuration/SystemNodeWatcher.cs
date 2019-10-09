// -----------------------------------------------------------------------
//  <copyright file="SystemNodeWatcher.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    public class SystemNodeWatcher
    {
        public SystemNodeWatcher(INode node)
        {
            Node = node;
        }

        public IList<double> OutputValues { get; } = new List<double>();
        public IList<double[]> InputValues { get; } = new List<double[]>();
        public INode Node { get; }

        public void AddValue(double output, IEnumerable<double> input)
        {
            OutputValues.Add(output);
            InputValues.Add(input.ToArray());
        }
    }
}