using Meadow.Hardware;
using Meadow.Modbus;
using System.Diagnostics;

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

            var i = 0;
            while (true)
            {
                await Task.Delay(1000);
                Debug.WriteLine($"Temp: {tstat.Temperature}");
                Debug.WriteLine($"SetPoint: {tstat.SetPoint}");
                Debug.WriteLine($"MinSetPoint: {tstat.MinSetPoint}");
                Debug.WriteLine($"MaxSetPoint: {tstat.MaxSetPoint}");
                Debug.WriteLine($"PowerUpSetPoint: {tstat.PowerUpSetPoint}");
                Debug.WriteLine($"Humidity: {tstat.Humidity}");
                Debug.WriteLine($"Clock: {tstat.Clock}");
            }
        }
    }
}