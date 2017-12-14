// -----------------------------------------------------------------------
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
        double GetValue(int index);
    }
}