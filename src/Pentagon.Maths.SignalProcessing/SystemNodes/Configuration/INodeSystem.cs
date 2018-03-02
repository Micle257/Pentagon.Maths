// -----------------------------------------------------------------------
//  <copyright file="INodeSystem.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using Abstractions;

    public interface INodeSystem
    {
        INode Output { get; }
        void ConfigureConnections(IConnectionBuilder builder);
        void ConfigureConstrains(SystemManagerConstrainCollection contrains);
        void Initialize();
    }
}