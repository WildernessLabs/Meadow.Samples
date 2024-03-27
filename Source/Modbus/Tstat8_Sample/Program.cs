using Meadow.Hardware;
using Meadow.Modbus;

namespace Tstat8_Sample;

internal class Program
{
    private static async Task Main(string[] _)
    {
        var serialPort = "COM3";
        byte thermostatModbusAddress = 201;

        using (var port = new SerialPortShim(serialPort, 19200, Parity.None, 8, StopBits.One))
        {
            port.ReadTimeout = TimeSpan.FromSeconds(15);
            port.Open();

            var client = new ModbusRtuClient(port);
            var tstat = new TStat8(client, thermostatModbusAddress, TimeSpan.FromSeconds(1));
            tstat.StartPolling();

            Console.WriteLine($"MinSetPoint: {tstat.MinSetPoint}");
            Console.WriteLine($"MaxSetPoint: {tstat.MaxSetPoint}");
            Console.WriteLine($"PowerUpSetPoint: {tstat.PowerUpSetPoint}");
            while (true)
            {
                await Task.Delay(1000);
                Console.WriteLine($"Temp: {tstat.Temperature}");
                Console.WriteLine($"SetPoint: {tstat.SetPoint}");
                Console.WriteLine($"Humidity: {tstat.Humidity}");
                Console.WriteLine($"Clock: {tstat.Clock}");
            }
        }
    }
}