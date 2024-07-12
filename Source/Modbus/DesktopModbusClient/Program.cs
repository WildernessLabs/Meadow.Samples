using Meadow.Modbus;
using System;
using System.Net;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Meadow Modbus Client");
        Console.WriteLine("Enter your Meadow IP address:");

        var input = Console.ReadLine();

        IPAddress? ip;

        if (!IPAddress.TryParse(input, out ip))
        {
            Console.WriteLine("Invalid IP Address!");
            return;
        }

        var client = new ModbusTcpClient(ip);
        Console.WriteLine($"Connecting to {ip}...");

        await client.Connect();

        var buffer = new byte[12];

        while (true)
        {
            var registers = await client.ReadInputRegisters(255, 0, 6);

            Console.WriteLine($"Read {registers.Length} registers");

            Buffer.BlockCopy(registers, 0, buffer, 0, registers.Length * 2);

            var temp = BitConverter.ToSingle(buffer, 0);
            var humidity = BitConverter.ToSingle(buffer, 4);
            var pressure = BitConverter.ToSingle(buffer, 8);
            Console.WriteLine($"Temp: {temp:0.00}C Humidity: {humidity:0.0}% Pressure: {pressure:0.0}Pa");

            await Task.Delay(2000);
        }
    }
}