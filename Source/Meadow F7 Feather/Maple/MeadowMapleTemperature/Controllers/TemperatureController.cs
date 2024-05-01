using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System;
using System.Collections.ObjectModel;

namespace MeadowMapleTemperature.Controllers;

public class TemperatureController
{
    private ITemperatureSensor analogTemperature;

    public ObservableCollection<TemperatureModel> TemperatureLogs { get; private set; }

    public TemperatureController() { }

    public void Initialize()
    {
        Resolver.Services.Add(this);

        TemperatureLogs = new ObservableCollection<TemperatureModel>();

        analogTemperature = new AnalogTemperature(
            MeadowApp.Device.Pins.A01,
            AnalogTemperature.KnownSensorType.LM35);
        analogTemperature.Updated += AnalogTemperatureUpdated;
        analogTemperature.StartUpdating(TimeSpan.FromSeconds(30));
    }

    private void AnalogTemperatureUpdated(object sender, IChangeResult<Temperature> e)
    {
        int TIMEZONE_OFFSET = -8;

        var ledController = Resolver.Services.Get<LedController>();
        ledController.SetColor(Color.Magenta);

        TemperatureLogs.Add(new TemperatureModel()
        {
            Temperature = e.New.Celsius.ToString("00"),
            DateTime = DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("yyyy-MM-dd hh:mm:ss tt")
        });

        ledController.SetColor(Color.Green);
    }
}