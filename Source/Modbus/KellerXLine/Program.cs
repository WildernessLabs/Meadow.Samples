using Meadow.Foundation.Sensors.Environmental;
using Meadow.Hardware;
using Meadow.Modbus;

namespace KellerXLine_Sample;

internal class Program
{
    private static async Task Main(string[] _)
    {
        await Test();
    }

    private static async Task Test()
    {

        var serialPort = "COM12";

        using (var port = new SerialPortShim(serialPort, KellerTransducer.DefaultBaudRate, Parity.None, 8, StopBits.One))
        {
            port.ReadTimeout = TimeSpan.FromSeconds(15);
            port.Open();

            var client = new ModbusRtuClient(port);
            var sensor = new KellerTransducer(client, KellerTransducer.DefaultModbusAddress);

            while (true)
            {
                var pressure = await sensor.ReadPressure(PressureChannel.P1);
                Console.WriteLine($"Pressure: {pressure.Millibar} mbar");

                await Task.Delay(1000);
            }
        }
    }
}