// -----------------------------------------------------------------------
//  <copyright file="SystemManagerConstrain.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    public class SystemManagerConstrain<TModel> : ISystemManagerConstrain
            where TModel : INodeSystem
    {
        readonly TModel _model;
        readonly Func<TModel, INode> _noteSelector;
        readonly Func<(double OutputValue, double[] InputValues), bool> _condition;
        readonly Action<TModel> _action;

        public SystemManagerConstrain(TModel model, Func<TModel, INode> noteSelector, Func<(double OutputValue, double[] InputValues), bool> condition, Action<TModel> action, bool forceCompute)
        {
            ForceCompute = forceCompute;
            _model = model;
            _condition = condition;
            _action = action;

            Node = noteSelector(_model);
        }

        public bool ForceCompute { get; }

        /// <inheritdoc />
        public INode Node { get; }

        public bool Run(double outputValue, IEnumerable<double> inputValues)
        {
            var result = _condition((outputValue, inputValues?.ToArray()));

            if (result)
                _action(_model); // TODO make togglable

            return result && ForceCompute;
        }
    }
}