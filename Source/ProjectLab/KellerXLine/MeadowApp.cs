using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Environmental;
using System;
using System.Threading.Tasks;

namespace KellerXLine_Sample;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private IKellerTransducer sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        var client = Hardware.GetModbusRtuClient(KellerTransducer.DefaultBaudRate);
        sensor = new KellerTransducer(client, KellerTransducer.DefaultModbusAddress);

        return base.Initialize();
    }

    public override async Task Run()
    {
        while (true)
        {
            try
            {
                var pressure = await sensor.ReadPressure(PressureChannel.P1);
                Resolver.Log.Info($"Pressure: {pressure.Millibar} mbar");
            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"Error: {ex.Message}");
            }

            await Task.Delay(1000);
        }
    }
}