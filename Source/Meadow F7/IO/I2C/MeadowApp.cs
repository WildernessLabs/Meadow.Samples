using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace I2C;

public class FeatherV1App : MeadowApp<F7FeatherV1> { }
public class FeatherV2App : MeadowApp<F7FeatherV2> { }
public class FeatherCcmV2App : MeadowApp<F7CoreComputeV2> { }

public abstract class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    private II2cBus i2c;
    private GY521 gyro;

    public override Task Initialize()
    {
        Resolver.Log.Info("+GY521 Speed Change Test");

        i2c = Device.CreateI2cBus();
        gyro = new GY521(i2c);
        gyro.Wake();

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        var count = 0;

        while (true)
        {
            try
            {
                Resolver.Log.Info($"Reading @{((int)i2c.BusSpeed / 1000d):0} kHz...");
                gyro.Refresh();

                Resolver.Log.Info($"({gyro.AccelerationX:X4},{gyro.AccelerationY:X4},{gyro.AccelerationZ:X4}) ({gyro.GyroX:X4},{gyro.GyroY:X4},{gyro.GyroZ:X4}) {gyro.Temperature}");

                switch (count++ % 4)
                {
                    case 0:
                        i2c.BusSpeed = I2cBusSpeed.Standard;
                        break;
                    case 1:
                        i2c.BusSpeed = I2cBusSpeed.Fast;
                        break;
                    case 2:
                        i2c.BusSpeed = I2cBusSpeed.FastPlus;
                        break;
                    case 3:
                        i2c.BusSpeed = I2cBusSpeed.High;
                        break;
                }
            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"Error: {ex.Message}");
            }

            await Task.Delay(2000);
        }
    }

    private async Task GY521Test()
    {
        var i2c = Device.CreateI2cBus();

        Resolver.Log.Info("+GY521 Test");

        var gyro = new GY521(i2c);

        Resolver.Log.Info("Wake");
        gyro.Wake();

        while (true)
        {
            Resolver.Log.Info("Reading...");
            gyro.Refresh();

            Resolver.Log.Info($"({gyro.AccelerationX:X4},{gyro.AccelerationY:X4},{gyro.AccelerationZ:X4}) ({gyro.GyroX:X4},{gyro.GyroY:X4},{gyro.GyroZ:X4}) {gyro.Temperature}");

            await Task.Delay(2000);
        }
    }

    private async Task BusScan(II2cBus i2c)
    {
        byte addr = 0;
        while (true)
        {
            if (++addr >= 127) addr = 1;

            Resolver.Log.Info($"Address: {addr}");

            i2c.Write(addr, new byte[] { 0 });
            await Task.Delay(2000);
        }
    }
}