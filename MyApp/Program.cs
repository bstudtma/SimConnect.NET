// <copyright file="Program.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>
using SimConnect.NET;

var client = new SimConnectClient();
await client.ConnectAsync();

// Get aircraft data in one shot using a SimVar-annotated struct
// var snapshot = await client.SimVars.GetAsync<MyApp.AircraftSnapshot>();
// Console.WriteLine($"Altitude: {snapshot.AltitudeFeet:F0} ft");
// Console.WriteLine($"Airspeed: {snapshot.IndicatedAirspeedKnots:F0} kts");
var snapshot = await client.SimVars.Test<MyApp.AircraftSnapshot>();
Console.WriteLine($"[Test] Altitude: {snapshot.AltitudeFeet:F0} ft");
Console.WriteLine($"[Test] Airspeed: {snapshot.IndicatedAirspeedKnots:F0} kts");
