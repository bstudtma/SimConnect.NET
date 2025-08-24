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

// Create a cancellation token source
var cts = new CancellationTokenSource();

// Subscribe to altitude updates with cancellation token
var altitudeDef = new SimConnect.NET.SimVar.SimVarDefinition("PLANE ALTITUDE", "feet", SimConnectDataType.FloatDouble, false, null);
Task subTask = client.SimVars.SubscribeAsync<double>(
    altitudeDef,
    0,
    SimConnectPeriod.Second,
    value => Console.WriteLine($"[Subscription] Altitude: {value:F0} ft"),
    cts.Token);

// Let the subscription run for 15 seconds
await Task.Delay(TimeSpan.FromSeconds(15), CancellationToken.None);

// Cancel the subscription
cts.Cancel();
await subTask;
