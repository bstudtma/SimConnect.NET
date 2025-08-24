// <copyright file="SimVarRequest.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimConnect.NET.SimVar
{
    /// <summary>
    /// Represents a pending SimVar request.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    public sealed class SimVarRequest<T>
    {
        private readonly TaskCompletionSource<T> taskCompletionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimVarRequest{T}"/> class.
        /// </summary>
        /// <param name="requestId">The unique request identifier.</param>
        /// <param name="definition">The SimVar definition.</param>
        /// <param name="objectId">The object ID (usually SIMCONNECT_OBJECT_ID_USER).</param>
        public SimVarRequest(uint requestId, SimVarDefinition definition, uint objectId)
        {
            this.RequestId = requestId;
            this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.ObjectId = objectId;
            this.taskCompletionSource = new TaskCompletionSource<T>();
            this.IsRecurring = false;
            this.OnValue = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimVarRequest{T}"/> class for recurring subscriptions.
        /// </summary>
        /// <param name="requestId">The unique request identifier.</param>
        /// <param name="definition">The SimVar definition.</param>
        /// <param name="objectId">The object ID (usually SIMCONNECT_OBJECT_ID_USER).</param>
        /// <param name="isRecurring">Whether this request is a recurring subscription.</param>
        /// <param name="onValue">Optional callback invoked for each received value when recurring.</param>
        public SimVarRequest(uint requestId, SimVarDefinition definition, uint objectId, bool isRecurring, Action<T>? onValue)
        {
            this.RequestId = requestId;
            this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.ObjectId = objectId;
            this.taskCompletionSource = new TaskCompletionSource<T>();
            this.IsRecurring = isRecurring;
            this.OnValue = onValue;
        }

        /// <summary>
        /// Gets the unique request identifier.
        /// </summary>
        public uint RequestId { get; }

        /// <summary>
        /// Gets the SimVar definition.
        /// </summary>
        public SimVarDefinition Definition { get; }

        /// <summary>
        /// Gets the object ID.
        /// </summary>
        public uint ObjectId { get; }

        /// <summary>
        /// Gets a value indicating whether this is a recurring subscription.
        /// </summary>
        public bool IsRecurring { get; }

        /// <summary>
        /// Gets the callback invoked for each received value when recurring.
        /// </summary>
        public Action<T>? OnValue { get; }

        /// <summary>
        /// Gets the task that completes when the request is fulfilled.
        /// </summary>
        public Task<T> Task => this.taskCompletionSource.Task;

        /// <summary>
        /// Completes the request with a successful result.
        /// </summary>
        /// <param name="result">The result value.</param>
        public void SetResult(T result)
        {
            if (this.IsRecurring)
            {
                try
                {
                    this.OnValue?.Invoke(result);
                }
                catch
                {
                    // Swallow exceptions from user callbacks to avoid breaking the message loop.
                }

                return;
            }

            this.taskCompletionSource.TrySetResult(result);
        }

        /// <summary>
        /// Completes the request with an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void SetException(Exception exception)
        {
            this.taskCompletionSource.TrySetException(exception);
        }

        /// <summary>
        /// Cancels the request.
        /// </summary>
        public void SetCanceled()
        {
            this.taskCompletionSource.TrySetCanceled();
        }
    }
}
