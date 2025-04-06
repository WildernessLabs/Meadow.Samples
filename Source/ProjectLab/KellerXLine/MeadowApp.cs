using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Environmental;
using Meadow.Hardware;
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

        return base.Initialize();
    }

    public override async Task Run()
    {
        var port = CreateSerialPort(KellerTransducer.DefaultBaudRate);
        var client = new Meadow.Modbus.ModbusRtuClient(port);

        var sensor = new KellerTransducer(client, KellerTransducer.DefaultModbusAddress);

        while (true)
        {
            await RunTestFormat2();

            await Task.Delay(1000);
        }
    }

    private async Task RunTestFormat2()
    {
        Resolver.Log.Info("...beginning RunFailingTest...");

        int loopCount = 2;

        do
        {
            var port = CreateSerialPort(KellerTransducer.DefaultBaudRate);
            port.Open();

            var client = new Meadow.Modbus.ModbusRtuClient(port);
            await client.Connect();

            var sensor = new KellerTransducer(client, KellerTransducer.DefaultModbusAddress);
            var pressure = await sensor.ReadPressure(PressureChannel.P1);
            Resolver.Log.Info($"Pressure: {pressure.Millibar} mbar");

            await Task.Delay(TimeSpan.FromSeconds(5));

            port.Close();
            port.Dispose();

        } while (loopCount++ <= 2);

        Resolver.Log.Info("RunFailingTest complete.");

    }

    private ISerialPort CreateSerialPort(int baudRate)
    {
        var portName = Hardware.ComputeModule.PlatformOS.GetSerialPortName("COM1");

        Resolver.Log.Trace($"Creating serial port {portName.FriendlyName}...");

        var port = Hardware.ComputeModule.CreateSerialPort(portName, baudRate, 8, Parity.None, StopBits.One);
        port.WriteTimeout = port.ReadTimeout = TimeSpan.FromSeconds(10);

        Resolver.Log.Trace($"Serial port {portName.FriendlyName} created.");

        return port;
    }
}