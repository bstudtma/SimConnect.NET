// <copyright file="SimVarAttribute.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;

namespace SimConnect.NET.SimVar
{
    /// <summary>Annotates a struct field with the SimVar you want marshalled into it.</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SimVarAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimVarAttribute"/> class.
        /// </summary>
        /// <param name="name">The SimVar name to marshal.</param>
        /// <param name="unit">The unit of the SimVar.</param>
        /// <param name="dataType">The SimConnect data type for marshaling.</param>
        /// <param name="order">The order in which the SimVar should be marshaled (optional).</param>
        public SimVarAttribute(string name, string unit, SimConnectDataType dataType, int order = 0)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.DataType = dataType;
            this.Order = order;
        }

        /// <summary>
        /// Gets the SimVar name to marshal.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the unit of the SimVar.
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// Gets the SimConnect data type for marshaling.
        /// </summary>
        public SimConnectDataType DataType { get; }

        /// <summary>
        /// Gets the order in which the SimVar should be marshaled.
        /// </summary>
        public int Order { get; }
    }
}
