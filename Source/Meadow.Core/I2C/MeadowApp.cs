using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace I2C
{
    public class MeadowApp : App<F7FeatherV2>
    {
        II2cBus i2c;
        GY521 gyro;

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

        async Task GY521Test()
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

        async Task BusScan(II2cBus i2c)
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

    public class GY521
    {
        private enum Registers : byte
        {
            PowerManagement = 0x6b,
            AccelerometerX = 0x3b,
            AccelerometerY = 0x3d,
            AccelerometerZ = 0x3f,
            Temperature = 0x41,
            GyroX = 0x43,
            GyroY = 0x45,
            GyroZ = 0x47
        }

        private II2cBus _bus;

        public byte Address { get; }

        public GY521(II2cBus bus, byte address = 0x68)
        {
            Address = address;
            _bus = bus;
        }

        public void Wake()
        {
            _bus.Write(Address, new byte[] { (byte)Registers.PowerManagement });
        }

        int c = 0;

        public void Refresh()
        {
            // tell it to send us 14 bytes (each value is 2-bytes), starting at 0x3b
            byte address = c++ % 10 == 0 ? (byte)(Address + 1) : Address;

            // cause occasional errors
            _bus.Write(address, new byte[] { (byte)Registers.AccelerometerX });
            var data = new byte[14];
            _bus.Write(address, data);

            //            Resolver.Log.Info($" Got {data.Length} bytes");
            //            Resolver.Log.Info($" {BitConverter.ToString(data)}");

            AccelerationX = data[0] << 8 | data[1];
            AccelerationY = data[2] << 8 | data[3];
            AccelerationZ = data[4] << 8 | data[5];
            Temperature = data[6] << 8 | data[7];
            GyroX = data[8] << 8 | data[9];
            GyroY = data[10] << 8 | data[11];
            GyroZ = data[12] << 8 | data[13];
        }

        public int AccelerationX { get; private set; }
        public int AccelerationY { get; private set; }
        public int AccelerationZ { get; private set; }
        public int Temperature { get; private set; }
        public int GyroX { get; private set; }
        public int GyroY { get; private set; }
        public int GyroZ { get; private set; }
    }
}
