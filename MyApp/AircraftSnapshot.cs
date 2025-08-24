// <copyright file="AircraftSnapshot.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using SimConnect.NET;
using SimConnect.NET.SimVar;

namespace MyApp
{
    /// <summary>
    /// Simple snapshot of commonly used aircraft telemetry requested in a single SimConnect call.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AircraftSnapshot
    {
        /// <summary>
        /// Gets or sets the pressure altitude of the aircraft in feet.
        /// </summary>
        [SimVar("PLANE ALTITUDE", "feet", SimConnectDataType.FloatDouble, order: 0)]
        public double AltitudeFeet;

        /// <summary>
        /// Gets or sets the indicated airspeed in knots.
        /// </summary>
        [SimVar("AIRSPEED INDICATED", "knots", SimConnectDataType.FloatDouble, order: 1)]
        public double IndicatedAirspeedKnots;
    }
}
