// <copyright file="Program.cs" company="BARS">
// Copyright (c) BARS. All rights reserved.
// </copyright>
using SimConnect.NET;

var client = new SimConnectClient();
await client.ConnectAsync();

// Get aircraft data
var altitude = await client.SimVars.GetAsync<double>("PLANE ALTITUDE", "feet");
var airspeed = await client.SimVars.GetAsync<double>("AIRSPEED INDICATED", "knots");

Console.WriteLine($"Altitude: {altitude:F0} ft");
Console.WriteLine($"Airspeed: {airspeed:F0} kts");
