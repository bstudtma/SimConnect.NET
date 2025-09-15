// <copyright file="SimVarSubscription.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using SimConnect.NET.SimVar.Internal; // Access internal ISimVarRequest

namespace SimConnect.NET.SimVar
{
    /// <summary>
    /// Default implementation of <see cref="ISimVarSubscription"/>.
    /// </summary>
    internal sealed class SimVarSubscription : ISimVarSubscription
    {
        private readonly SimVarManager manager;
        private readonly ISimVarRequest request;
        private readonly CancellationTokenRegistration cancellationRegistration;
        private int disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimVarSubscription"/> class.
        /// </summary>
        /// <param name="manager">Owning manager.</param>
        /// <param name="request">Underlying request instance.</param>
        /// <param name="cancellationToken">Optional cancellation token to auto-dispose.</param>
        public SimVarSubscription(SimVarManager manager, ISimVarRequest request, CancellationToken cancellationToken)
        {
            this.manager = manager;
            this.request = request;

            if (cancellationToken.CanBeCanceled)
            {
                this.cancellationRegistration = cancellationToken.Register(
                    static state =>
                    {
                        var self = (SimVarSubscription)state!;
                        self.Dispose();
                    },
                    this);
            }
        }

        /// <inheritdoc />
        public Task Completion => this.request.Completion;

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.Exchange(ref this.disposed, 1) != 0)
            {
                return;
            }

            this.manager.CancelRequest(this.request);
            this.cancellationRegistration.Dispose();
        }
    }
}
