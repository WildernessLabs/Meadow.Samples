using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort_Echo;

public class MeadowApp : App<F7FeatherV2>
{
    ISerialPort port;

    public override Task Initialize()
    {
        Resolver.Log.Info("Available serial ports:");
        foreach (var name in Device.PlatformOS.GetSerialPortNames())
        {
            Resolver.Log.Info($"  {name.FriendlyName}");
        }
        var serialPortName = Device.PlatformOS.GetSerialPortName("COM1");
        Resolver.Log.Info($"Using {serialPortName.FriendlyName}...");
        port = Device.CreateSerialPort(serialPortName, 115200);
        Resolver.Log.Info("\tCreated");
        port.Open();
        if (port.IsOpen)
        {
            Resolver.Log.Info("\tOpened");
        }
        else
        {
            Resolver.Log.Info("\tFailed to Open");
        }

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        var buffer = new byte[1024];

        while (true)
        {
            Resolver.Log.Info("Writing data...");
            port.Write(Encoding.ASCII.GetBytes("Hello Meadow!"));

            var dataLength = port.BytesToRead;
            var read = port.Read(buffer, 0, dataLength);

            if (read == 0)
            {
                Resolver.Log.Info($"Read {read} bytes");
            }
            else
            {
                Resolver.Log.Info($"Read {read} bytes: {BitConverter.ToString(buffer, 0, read)}");
            }

            await Task.Delay(2000);
        }
    }
}