using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Units;
using MeadowBleTemperature.Connectivity;
using System;

namespace MeadowBleTemperature.Controllers;

public class TemperatureController
{
    private AnalogTemperature analogTemperature;

    public TemperatureController()
    {
        analogTemperature = new AnalogTemperature(
            MeadowApp.Device.Pins.A01,
            AnalogTemperature.KnownSensorType.LM35);
        analogTemperature.Updated += AnalogTemperatureUpdated;
    }

    private void AnalogTemperatureUpdated(object sender, IChangeResult<Temperature> e)
    {
        Resolver.Services.Get<BluetoothServer>().UpdateTemperatureCharacteristic(e.New);
    }

    public void StartUpdating(TimeSpan timeSpan)
    {
        analogTemperature.StartUpdating(timeSpan);
    }
}