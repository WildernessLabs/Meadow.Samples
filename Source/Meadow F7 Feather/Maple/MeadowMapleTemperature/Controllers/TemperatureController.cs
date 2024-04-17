using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System;
using System.Collections.ObjectModel;

namespace MeadowMapleTemperature.Controllers;

public class TemperatureController
{
    ITemperatureSensor analogTemperature;

    private static readonly Lazy<TemperatureController> instance =
        new Lazy<TemperatureController>(() => new TemperatureController());
    public static TemperatureController Instance => instance.Value;

    public ObservableCollection<TemperatureModel> TemperatureLogs { get; private set; }

    private TemperatureController() { }

    public void Initialize()
    {
        TemperatureLogs = new ObservableCollection<TemperatureModel>();

        analogTemperature = new AnalogTemperature(MeadowApp.Device.Pins.A01,
            AnalogTemperature.KnownSensorType.LM35);
        analogTemperature.Updated += AnalogTemperatureUpdated;
        analogTemperature.StartUpdating(TimeSpan.FromSeconds(30));
    }

    private void AnalogTemperatureUpdated(object sender, Meadow.IChangeResult<Temperature> e)
    {
        int TIMEZONE_OFFSET = -8;

        LedController.Instance.SetColor(Color.Magenta);

        TemperatureLogs.Add(new TemperatureModel()
        {
            Temperature = e.New.Celsius.ToString("00"),
            DateTime = DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("yyyy-MM-dd hh:mm:ss tt")
        });

        LedController.Instance.SetColor(Color.Green);
    }
}