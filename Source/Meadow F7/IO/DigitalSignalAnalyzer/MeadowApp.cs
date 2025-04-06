using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace SignalAnalyzer;

/// <summary>
/// This sample illustrates using the IDigitalSignalAnalyzer.
/// </summary>
public class MeadowApp : App<F7FeatherV2>
{
    private IDigitalSignalAnalyzer input;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing hardware...");

        //==== create an analyzer of D06
        input = Device.CreateDigitalSignalAnalyzer(Device.Pins.D08);

        Resolver.Log.Info($"Analyzer is a {input.GetType().Name}");

        Resolver.Log.Info("Hardware initialized.");

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        var i = 0;

        while (true)
        {
            Resolver.Log.Info($"{i++} Frequency: {input.GetFrequency().Hertz:N1} DC: {input.GetDutyCycle():N2} AVG:{input.GetMeanFrequency().Hertz:N1}");

            await Task.Delay(2500);
        }
    }
}