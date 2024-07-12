using Meadow.Modbus;
using System.Net;

namespace ModbusClient;

internal class Program
{
    private static async Task Main(string[] _)
    {
        var ip = IPEndPoint.Parse("192.168.1.74:502");

        var client = new ModbusTcpClient(ip);
        client.Connect();

        await client.WriteMultipleCoils(1, 0, new bool[] { true, false, true, false });


        //Console.WriteLine($"MinSetPoint: {tstat.MinSetPoint}");
        //Console.WriteLine($"MaxSetPoint: {tstat.MaxSetPoint}");
        //Console.WriteLine($"PowerUpSetPoint: {tstat.PowerUpSetPoint}");
        while (true)
        {
            //var e = await client.ReadHoldingRegisters(1, 0, 4);
            var e = await client.ReadCoils(1, 0, 4);

            if (e.Length > 0)
                Console.WriteLine($"{e[0]} {e[1]} {e[2]} {e[3]}");

            await Task.Delay(1000);
            //Console.WriteLine($"Temp: {tstat.Temperature}");
            //Console.WriteLine($"SetPoint: {tstat.SetPoint}");
            //Console.WriteLine($"Humidity: {tstat.Humidity}");
            //Console.WriteLine($"Clock: {tstat.Clock}");
        }
    }
}