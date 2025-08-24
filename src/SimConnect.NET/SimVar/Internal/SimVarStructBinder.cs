// <copyright file="SimVarStructBinder.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Reflection;

namespace SimConnect.NET.SimVar.Internal
{
    internal static class SimVarStructBinder
    {
        /// <summary>
        /// Returns the ordered [SimVar]-annotated fields for T, validating .NET types vs SimConnect types.
        /// </summary>
    internal static (System.Reflection.FieldInfo Field, SimVarAttribute Attr)[] GetOrderedFields<T>()
    {
            var t = typeof(T);
            if (!t.IsLayoutSequential)
            {
                throw new InvalidOperationException($"{t.Name} must be annotated with [StructLayout(LayoutKind.Sequential)].");
            }

            var fields = t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                  .Select(f => (Field: f, Attr: f.GetCustomAttribute<SimVarAttribute>()))
                  .Where(x => x.Attr != null)
                  .OrderBy(x => x!.Attr!.Order)
                  .ThenBy(x => x.Field.MetadataToken)
                  .ToArray();

            if (fields.Length == 0)
            {
                throw new InvalidOperationException($"{t.Name} has no fields annotated with [SimVar].");
            }

            foreach (var (field, attr) in fields)
            {
                var ft = field.FieldType;
                switch (attr!.DataType)
                {
                    case SimConnectDataType.FloatDouble:
                        if (ft != typeof(double))
                        {
                            throw Fail(field, "double");
                        }

                        break;
                    case SimConnectDataType.FloatSingle:
                        if (ft != typeof(float))
                        {
                            throw Fail(field, "float");
                        }

                        break;
                    case SimConnectDataType.Integer32:
                        if (ft != typeof(int) && ft != typeof(uint))
                        {
                            throw Fail(field, "int/uint");
                        }

                        break;
                    case SimConnectDataType.Integer64:
                        if (ft != typeof(long) && ft != typeof(ulong))
                        {
                            throw Fail(field, "long/ulong");
                        }

                        break;
                    case SimConnectDataType.String8 or SimConnectDataType.String32 or SimConnectDataType.String64 or SimConnectDataType.String128 or SimConnectDataType.String256 or SimConnectDataType.String260:
                        if (ft != typeof(string))
                        {
                            throw Fail(field, "string");
                        }

                        break;
                }
            }

            return fields!;

            static InvalidOperationException Fail(FieldInfo f, string expected)
                => new($"Field {f.DeclaringType!.Name}.{f.Name} must be {expected} to match its [SimVar] attribute.");
        }

    /// <summary>
    /// Builds a single SimConnect data definition for T (using [SimVar] attributes),
    /// registers T for marshalling, and returns the definition ID.
    /// </summary>
    /// <param name="handle">Native SimConnect handle.</param>
    internal static (uint DefId, int ManagedSize) BuildAndRegisterFromStruct<T>(IntPtr handle)
        {
            var t = typeof(T);
            if (!t.IsLayoutSequential)
            {
                throw new InvalidOperationException($"{t.Name} must be annotated with [StructLayout(LayoutKind.Sequential)].");
            }

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                  .Select(f => (Field: f, Attr: f.GetCustomAttribute<SimVarAttribute>()))
                  .Where(x => x.Attr != null)
                  .OrderBy(x => x!.Attr!.Order)
                  .ThenBy(x => x.Field.MetadataToken)
                  .ToArray();

            if (fields.Length == 0)
            {
                throw new InvalidOperationException($"{t.Name} has no fields annotated with [SimVar].");
            }

            // Basic .NET ↔ SimConnect type validation to fail fast
            foreach (var (field, attr) in fields)
            {
                var ft = field.FieldType;
                switch (attr!.DataType)
                {
                    case SimConnectDataType.FloatDouble:
                        if (ft != typeof(double))
                        {
                            throw Fail(field, "double");
                        }

                        break;
                    case SimConnectDataType.FloatSingle:
                        if (ft != typeof(float))
                        {
                            throw Fail(field, "float");
                        }

                        break;
                    case SimConnectDataType.Integer32:
                        if (ft != typeof(int) && ft != typeof(uint))
                        {
                            throw Fail(field, "int/uint");
                        }

                        break;
                    case SimConnectDataType.Integer64:
                        if (ft != typeof(long) && ft != typeof(ulong))
                        {
                            throw Fail(field, "long/ulong");
                        }

                        break;
                    case SimConnectDataType.String8:
                    case SimConnectDataType.String32:
                    case SimConnectDataType.String64:
                    case SimConnectDataType.String128:
                    case SimConnectDataType.String256:
                    case SimConnectDataType.String260:
                        if (ft != typeof(string))
                        {
                            throw Fail(field, "string");
                        }

                        break;
                }
            }

            uint defId = unchecked((uint)Guid.NewGuid().GetHashCode());

            foreach (var (field, attr) in fields)
            {
                // Add each SimVar field to the SimConnect data definition using the native layer
                var result = SimConnectNative.SimConnect_AddToDataDefinition(
                    handle,
                    defId,
                    attr!.Name,
                    attr.Unit,
                    (uint)attr.DataType);
            }

            var size = System.Runtime.InteropServices.Marshal.SizeOf<T>();
            return (defId, size);

            static InvalidOperationException Fail(FieldInfo f, string expected)
                => new($"Field {f.DeclaringType!.Name}.{f.Name} must be {expected} to match its [SimVar] attribute.");
        }
    }
}
