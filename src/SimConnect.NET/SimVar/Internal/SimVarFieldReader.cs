// <copyright file="SimVarFieldReader.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace SimConnect.NET.SimVar.Internal
{
    internal delegate void Setter<TStruct, TValue>(ref TStruct target, TValue value)
        where TStruct : struct;  // necessary for TStruct parameter by ref, Action cannot express by ref

    internal sealed class SimVarFieldReader<T, TDest> : IFieldReader<T>
        where T : struct
    {
        public int OffsetBytes { get; set; }

        public int Size { get; set; }

        public SimConnectDataType DataType { get; set; }

        public Setter<T, TDest> Setter { get; set; } = default!;

        // Holds a typed converter matching the raw type for the chosen Kind, e.g. Func<double, TDest>.
        public Delegate Converter { get; set; } = default!;

        public void ReadInto(ref T target, IntPtr basePtr)
        {
            var addr = IntPtr.Add(basePtr, this.OffsetBytes);
            switch (this.DataType)
            {
                case SimConnectDataType.FloatDouble:
                    double rawDouble = SimVarMemoryReader.ReadDouble(addr);
                    var convDouble = (Func<double, TDest>)this.Converter;
                    this.Setter(ref target, convDouble(rawDouble));
                    break;

                case SimConnectDataType.FloatSingle:
                    float rawFloat = SimVarMemoryReader.ReadFloat(addr);
                    var convFloat = (Func<float, TDest>)this.Converter;
                    this.Setter(ref target, convFloat(rawFloat));
                    break;

                case SimConnectDataType.Integer64:
                    long rawInt64 = SimVarMemoryReader.ReadInt64(addr);
                    var convInt64 = (Func<long, TDest>)this.Converter;
                    this.Setter(ref target, convInt64(rawInt64));
                    break;

                case SimConnectDataType.Integer32:
                    int rawInt32 = SimVarMemoryReader.ReadInt32(addr);
                    var convInt32 = (Func<int, TDest>)this.Converter;
                    this.Setter(ref target, convInt32(rawInt32));
                    break;

                case SimConnectDataType.String8:
                case SimConnectDataType.String32:
                case SimConnectDataType.String64:
                case SimConnectDataType.String128:
                case SimConnectDataType.String256:
                case SimConnectDataType.String260:
                    string rawString = SimVarMemoryReader.ReadFixedString(addr, this.Size);
                    var convString = (Func<string, TDest>)this.Converter;
                    this.Setter(ref target, convString(rawString));
                    break;

                case SimConnectDataType.InitPosition:
                    var rawInit = Marshal.PtrToStructure<SimConnectDataInitPosition>(addr);
                    var convInit = (Func<SimConnectDataInitPosition, TDest>)this.Converter;
                    this.Setter(ref target, convInit(rawInit));
                    break;

                case SimConnectDataType.MarkerState:
                    var rawMarker = Marshal.PtrToStructure<SimConnectDataMarkerState>(addr);
                    var convMarker = (Func<SimConnectDataMarkerState, TDest>)this.Converter;
                    this.Setter(ref target, convMarker(rawMarker));
                    break;

                case SimConnectDataType.Waypoint:
                    var rawWp = Marshal.PtrToStructure<SimConnectDataWaypoint>(addr);
                    var convWp = (Func<SimConnectDataWaypoint, TDest>)this.Converter;
                    this.Setter(ref target, convWp(rawWp));
                    break;

                case SimConnectDataType.LatLonAlt:
                    var rawLatLonAlt = Marshal.PtrToStructure<SimConnectDataLatLonAlt>(addr);
                    var convLatLonAlt = (Func<SimConnectDataLatLonAlt, TDest>)this.Converter;
                    this.Setter(ref target, convLatLonAlt(rawLatLonAlt));
                    break;

                case SimConnectDataType.Xyz:
                    var rawXyz = Marshal.PtrToStructure<SimConnectDataXyz>(addr);
                    var convXyz = (Func<SimConnectDataXyz, TDest>)this.Converter;
                    this.Setter(ref target, convXyz(rawXyz));
                    break;

                default:
                    throw new NotSupportedException($"Unsupported SimConnectDataType {this.DataType}");
            }
        }
    }
}
