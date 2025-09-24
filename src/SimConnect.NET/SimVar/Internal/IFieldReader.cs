// <copyright file="IFieldReader.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;

namespace SimConnect.NET.SimVar.Internal
{
    /// <summary>
    /// Reads and assigns a value of type <typeparamref name="T"/> from native memory into a managed struct.
    /// </summary>
    /// <typeparam name="T">The value type being populated.</typeparam>
    /// <remarks>
    /// Renamed from IFieldAccessor to IFieldReader to better reflect its one-way responsibility (read/copy only).
    /// Design note: generic only on <typeparamref name="T"/> so callers can hold a heterogeneous collection of
    /// per-field readers without knowing each destination field's exact type parameter.
    /// </remarks>
    internal interface IFieldReader<T>
        where T : struct
    {
        /// <summary>
    /// Reads from the specified native base pointer plus the reader's offset and writes the value into the target.
        /// </summary>
        /// <param name="target">The struct instance being populated.</param>
        /// <param name="basePtr">The base pointer to the native buffer.</param>
    void ReadInto(ref T target, IntPtr basePtr);
    }
}
