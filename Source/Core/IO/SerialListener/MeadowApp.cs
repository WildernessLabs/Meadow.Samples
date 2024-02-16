using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SerialListener
{
    public class MeadowApp : App<F7FeatherV2>
    {
        ISerialPort classicSerialPort;

        public override Task Initialize()
        {
            // instantiate our serial port
            Resolver.Log.Info("Available serial ports:");
            foreach (var name in Device.PlatformOS.GetSerialPortNames())
            {
                Resolver.Log.Info($"  {name.FriendlyName}");
            }
            var serialPortName = Device.PlatformOS.GetSerialPortName("COM4");
            Resolver.Log.Info($"Using {serialPortName.FriendlyName}...");
            classicSerialPort = Device.CreateSerialPort(serialPortName, 9600);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            classicSerialPort.Open();
            Resolver.Log.Info("\tOpened");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            byte[] buffer = new byte[1024];
            int bytesToRead;

            // polls the serial and outputs anything
            // in the buffer.
            while (true)
            {
                bytesToRead = classicSerialPort.BytesToRead;
                if (bytesToRead > buffer.Length)
                {
                    bytesToRead = buffer.Length;
                }
                int dataLength = classicSerialPort.Read(buffer, 0, bytesToRead);

                if (dataLength > 0)
                {
                    Resolver.Log.Info(ParseToString(buffer, dataLength, Encoding.ASCII));
                }

                await Task.Delay(500);
            }
        }

        /// <summary>
        /// C# compiler doesn't allow Span<T> in async methods, so can't do this
        /// inline.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        protected string ParseToString(byte[] buffer, int length, Encoding encoding)
        {
            Span<byte> actualData = buffer.AsSpan<byte>().Slice(0, length);
            return encoding.GetString(actualData);
        }
    }
}