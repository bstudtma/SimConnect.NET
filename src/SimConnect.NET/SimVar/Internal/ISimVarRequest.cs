// <copyright file="ISimVarRequest.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace SimConnect.NET.SimVar.Internal
{
    /// <summary>
    /// Non-generic SimVar request contract for hot-path handling without reflection.
    /// </summary>
    internal interface ISimVarRequest
    {
        /// <summary>
        /// Gets the unique request identifier.
        /// </summary>
        uint RequestId { get; }

        /// <summary>
        /// Gets the SimConnect object identifier targeted by this request.
        /// </summary>
        uint ObjectId { get; }

        /// <summary>
        /// Gets the data definition identifier associated with this request.
        /// </summary>
        uint DefinitionId { get; }

        /// <summary>
        /// Gets a value indicating whether this is a recurring subscription.
        /// </summary>
        bool IsRecurring { get; }

        /// <summary>
        /// Gets a task that completes when the request finishes (result, cancel, or error). For recurring requests this completes only on cancel/error.
        /// </summary>
        Task Completion { get; }

        /// <summary>
        /// Sets the result using a boxed value; the implementation converts to the expected T.
        /// </summary>
        /// <param name="value">The boxed value parsed from SimConnect.</param>
        void SetResultBoxed(object? value);

        /// <summary>
        /// Completes the request with an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        void SetException(Exception exception);

        /// <summary>
        /// Cancels the request.
        /// </summary>
        void SetCanceled();

        /// <summary>
        /// Attempts to complete the request with the provided typed value without boxing.
        /// Returns true only if the request's expected type matches <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the provided value.</typeparam>
        /// <param name="value">The value to set.</param>
        /// <returns>True if the value type matches and was accepted; otherwise false.</returns>
        bool TrySetResult<TValue>(TValue value);
    }
}
