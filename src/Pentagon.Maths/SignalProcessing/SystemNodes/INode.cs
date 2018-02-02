﻿// -----------------------------------------------------------------------
//  <copyright file="INode.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes
{
    using System.Collections.Generic;
    using Functions;

    public interface INode
    {
        string Name { get; set; }
        double GetValue(int index);
        double GetValue(int index, params double[] inputValues);
        int InputCount { get; }
    }
}