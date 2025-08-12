using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Modbus;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using MeadowModbusServer.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MeadowModbusServer;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private IProjectLabHardware projectLab;
    private ModbusTcpServer modbusServer;
    private RegisterBank registers;
    private DisplayScreen screen;
    private Label addressLabel;
    private Label telemetryLabel;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        // map sensor values to input registers
        registers = new RegisterBank((int)RegisterBank.Registers.TotalLength);

        projectLab = Hardware;

        BuildScreen();

        projectLab.BarometricPressureSensor.Updated += BarometricPressureSensor_Updated;
        projectLab.BarometricPressureSensor.StartUpdating();

        var wifi = Hardware.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        if (wifi.IsConnected)
        {
            StartModbusServer(wifi.IpAddress);
        }
        else
        {
            wifi.NetworkConnected += OnNetworkConnected;
        }

        return Task.CompletedTask;
    }

    private void OnNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        StartModbusServer(args.IpAddress);
    }

    private void StartModbusServer(IPAddress ipAddress)
    {
        Resolver.Log.Info($"Network connected. Starting Modbus server on {ipAddress}");

        // update the display to make connecting easier
        ShowNetworkAddress(ipAddress);

        // set up modbus server
        modbusServer = new ModbusTcpServer();
        modbusServer.ReadInputRegisterRequest += OnModbusServerReadInputRegisterRequest;
        modbusServer.ClientConnected += OnClientConnected;

        modbusServer.Start();
    }

    private void BuildScreen()
    {
        var theme = new DisplayTheme
        {
            Font = new Font12x20()
        };

        screen = new DisplayScreen(projectLab.Display, RotationType._270Degrees, theme: theme);

        var header = new Label(10, 10, screen.Width, 20);
        header.Text = "Meadow Modbus Server";
        addressLabel = new Label(10, 40, screen.Width, 20);
        addressLabel.Text = "Connecting...";
        telemetryLabel = new Label(10, 70, screen.Width, 20);
        telemetryLabel.Font = new Font8x16();

        screen.Controls.Add(header, addressLabel, telemetryLabel);
    }

    private IModbusResult OnModbusServerReadInputRegisterRequest(byte modbusAddress, ushort startRegister, short length)
    {
        Resolver.Log.Info($"Read request for {length} registers starting at {startRegister}");

        try
        {
            if (!Enum.IsDefined(typeof(RegisterBank.Registers), startRegister))
            {
                Resolver.Log.Info($"Illegal address");
                return new ModbusErrorResult(ModbusErrorCode.IllegalDataAddress);
            }

            var r = registers.GetRegisters(startRegister, length);
            if (r == null)
            {
                Resolver.Log.Info($"Illegal offset");
                return new ModbusErrorResult(ModbusErrorCode.InvalidOffset);
            }

            return new ModbusReadResult(r);
        }
        catch (Exception ex)
        {
            Resolver.Log.Info($"{ex.Message}");
            return new ModbusErrorResult(ModbusErrorCode.IllegalFunction);
        }
    }

    private void OnClientConnected(object sender, EndPoint e)
    {
        Resolver.Log.Info($"Client connected from {e}");
    }

    private void ShowNetworkAddress(IPAddress address)
    {
        addressLabel.Text = address.ToString();
    }


    private void BarometricPressureSensor_Updated(object sender, IChangeResult<Pressure> e)
    {
        var d = new Dictionary<string, float>();

        // we lose some numeric accuracy here, but 64-bits is much higher precision than the sensor
        if (Hardware.TemperatureSensor != null)
        {
            registers.SetRegisters(RegisterBank.Registers.Temperature, (float)Hardware.TemperatureSensor.Read().Result.Celsius);
            d.Add("Temp", (float)Hardware.TemperatureSensor.Read().Result.Celsius);
        }
        if (Hardware.HumiditySensor != null)
        {
            registers.SetRegisters(RegisterBank.Registers.Humidity, (float)Hardware.HumiditySensor.Humidity.Value.Percent);
            d.Add("Hum", (float)Hardware.HumiditySensor.Humidity.Value.Percent);
        }
        if (Hardware.BarometricPressureSensor != null)
        {
            registers.SetRegisters(RegisterBank.Registers.AirPressure, (float)Hardware.BarometricPressureSensor.Pressure.Value.Pascal);
            d.Add("Pres", (float)Hardware.BarometricPressureSensor.Pressure.Value.Pascal);
        }

        ShowTelemetry(d);
    }

    private void ShowTelemetry(IEnumerable<KeyValuePair<string, float>> telemetry)
    {
        telemetryLabel.Text = string.Join("  ",
            telemetry.Select(t => $"{t.Key}={t.Value:0.0}"));
    }
}