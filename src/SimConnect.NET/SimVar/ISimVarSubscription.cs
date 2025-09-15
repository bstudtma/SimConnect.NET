// <copyright file="ISimVarSubscription.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace SimConnect.NET.SimVar
{
    /// <summary>
    /// Represents an active SimVar subscription that can be disposed to stop receiving data.
    /// </summary>
    public interface ISimVarSubscription : IDisposable
    {
        /// <summary>
        /// Gets a task that completes when the subscription terminates (via dispose, cancellation, or error).
        /// </summary>
        Task Completion { get; }
    }
}
