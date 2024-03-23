using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Modbus;
using Meadow.Peripherals.Displays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MeadowModbusServer;

public class MeadowApp : App<F7CoreComputeV2>
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

        projectLab = ProjectLab.Create();

        BuildScreen();

        (projectLab as ProjectLabHardwareBase).AtmosphericSensor.Updated += OnEnvironmentalSensorUpdated;
        (projectLab as ProjectLabHardwareBase).AtmosphericSensor.StartUpdating();

        Device.NetworkConnected += OnNetworkConnected;

        _ = Device
            .NetworkAdapters
            .Primary<IWiFiNetworkAdapter>()
            .Connect("BOBS_YOUR_UNCLE", "1234567890");

        return Task.CompletedTask;
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

    private void OnNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        Resolver.Log.Info($"Network connected.  Starting Modbus server on {args.IpAddress}");

        // update the display to make connecting easier
        ShowNetworkAddress(args.IpAddress);

        // set up modbus server
        modbusServer = new ModbusTcpServer();
        modbusServer.ReadInputRegisterRequest += OnModbusServerReadInputRegisterRequest;
        modbusServer.ClientConnected += OnClientConnected;

        modbusServer.Start();
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

    private void ShowTelemetry(IEnumerable<KeyValuePair<string, float>> telemetry)
    {
        telemetryLabel.Text = string.Join("  ",
            telemetry.Select(t => $"{t.Key}={t.Value:0.0}"));
    }

    private void OnEnvironmentalSensorUpdated(object sender, IChangeResult<(Meadow.Units.Temperature? Temperature, Meadow.Units.RelativeHumidity? Humidity, Meadow.Units.Pressure? Pressure, Meadow.Units.Resistance? GasResistance)> e)
    {
        var d = new Dictionary<string, float>();

        // we lose some numeric accuracy here, but 64-bits is much higher precision than the sensor
        if (e.New.Temperature != null)
        {
            registers.SetRegisters(RegisterBank.Registers.Temperature, (float)e.New.Temperature.Value.Celsius);
            d.Add("Temp", (float)e.New.Temperature.Value.Celsius);
        }
        if (e.New.Humidity != null)
        {
            registers.SetRegisters(RegisterBank.Registers.Humidity, (float)e.New.Humidity.Value.Percent);
            d.Add("Hum", (float)e.New.Humidity.Value.Percent);
        }
        if (e.New.Pressure != null)
        {
            registers.SetRegisters(RegisterBank.Registers.AirPressure, (float)e.New.Pressure.Value.Pascal);
            d.Add("Pres", (float)e.New.Pressure.Value.Pascal);
        }

        ShowTelemetry(d);
    }
}