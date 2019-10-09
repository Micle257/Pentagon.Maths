// -----------------------------------------------------------------------
//  <copyright file="SystemManagerConstrainCollection.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System;
    using System.Collections.Generic;
    using Abstractions;

    public class SystemManagerConstrainCollection
    {
        public List<ISystemManagerConstrain> Items { get; } = new List<ISystemManagerConstrain>();

        public void Add<TModel>(TModel model, Func<TModel, INode> noteSelector, Func<(double OutputValue, double[] InputValues), bool> condition, Action<TModel> action, bool forceCompute)
                where TModel : INodeSystem
        {
            Items.Add(new SystemManagerConstrain<TModel>(model, noteSelector, condition, action, forceCompute));
        }
    }
}