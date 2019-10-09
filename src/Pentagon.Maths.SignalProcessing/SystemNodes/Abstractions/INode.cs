// -----------------------------------------------------------------------
//  <copyright file="INode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Abstractions
{
    public interface INode
    {
        string Name { get; set; }
        double GetValue(int index, params double[] inputValues);
    }
}