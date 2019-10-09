// -----------------------------------------------------------------------
//  <copyright file="ConnectionBuilderExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing.SystemNodes.Configuration
{
    using Abstractions;

    public static class ConnectionBuilderExtensions
    {
        public static IConnectionBuilder Connect(this IConnectionBuilder builder, INode node, INodeSystem system)
        {
            builder.Connect(node, system.Output);

            return builder;
        }
    }
}