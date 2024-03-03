using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort
{
    /// <summary>
    /// To run these tests, create a loopback on COM4 by connecting D12 and D13.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        ISerialPort classicSerialPort;
        Encoding currentTestEncoding = Encoding.ASCII;

        public override Task Initialize()
        {
            Resolver.Log.Info("Available serial ports:");
            foreach (var name in Device.PlatformOS.GetSerialPortNames())
            {
                Resolver.Log.Info($"  {name.FriendlyName}");
            }
            var serialPortName = Device.PlatformOS.GetSerialPortName("COM1");
            Resolver.Log.Info($"Using {serialPortName.FriendlyName}...");
            classicSerialPort = Device.CreateSerialPort(serialPortName, 115200);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            classicSerialPort.Open();
            Resolver.Log.Info("\tOpened");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Resolver.Log.Info("BUGBUG: this test fails under specific conditions. See test for info.");
            SimpleReadWriteTest();
            Resolver.Log.Info("Simple read/write testing completed.");

            await SerialEventTest();
            Resolver.Log.Info("Serial event testing completed.");

            Resolver.Log.Info("LongMessageTest - currently, not absolutely sure this works. Console.WriteLine might be clipping output.");
            Resolver.Log.Info("Also, test it with unicode encoding and things go sideways, are we losing a byte?? ");
            await LongMessageTest();
        }

        /// <summary>
        /// Tests basic reading of serial in which the Write.Length == Read.Count
        /// </summary>
        async Task SimpleReadWriteTest()
        {
            int count = 10;
            currentTestEncoding = Encoding.Unicode;

            //Span<byte> buffer = new byte[512];
            byte[] buffer = new byte[512];

            // run the test a few times
            int dataLength = 0;
            for (int i = 0; i < 10; i++)
            {
                Resolver.Log.Info("Writing data...");
                /*dataLength =*/
                classicSerialPort.Write(currentTestEncoding.GetBytes($"{count * i} PRINT Hello Meadow!"));

                // give some time for the electrons to electronify
                // TODO/HACK/BUGBUG: reduce this to 100ms and weird stuff happens;
                // specifically we get the following output, and i don't know why:
                // Writing data...
                // Serial data: 0 PRINT Hello Meadow!
                // Writing data...
                // Serial data: 0 PRINT Hello Meadow!
                // Writing data...
                // Serial data: 10 PRINT Hello Meadow!
                // Writing data...
                // Serial data: 20 PRINT Hello Meadow!
                // ...
                // how is it possible that the first line is there twice, even
                // though we're clearing it out??
                await Task.Delay(300);

                // empty it out
                dataLength = classicSerialPort.BytesToRead;
                classicSerialPort.Read(buffer, 0, dataLength);

                Resolver.Log.Info($"Serial data: {ParseToString(buffer, dataLength, currentTestEncoding)}");

                await Task.Delay(300);
            }
        }

        // TODO: Someone smarter than me (bryan) needs to review this and determine
        // if my use of Span<T> is actually saving anything here.
        async Task SerialEventTest()
        {
            Resolver.Log.Info("SerialEventTest");

            currentTestEncoding = Encoding.Unicode;
            classicSerialPort.DataReceived += ProcessData;

            // send some messages
            await Task.Run(async () =>
            {
                Resolver.Log.Info("Sending 8 messages of profundity.");
                classicSerialPort.Write(currentTestEncoding.GetBytes("Ticking away the moments that make up a dull day,"));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("fritter and waste the hours in an offhand way."));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("Kicking around on a piece of ground in your home town,"));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("Waiting for someone or something to show you the way."));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("Tired of lying in the sunshine, staying home to watch the rain,"));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("you are young and life is long and there is time to kill today."));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("And then one day you find ten years have got behind you,"));
                await Task.Delay(100);
                classicSerialPort.Write(currentTestEncoding.GetBytes("No one told you when to run, you missed the starting gun."));
                await Task.Delay(100);
            });

            //weak ass Hack to wait for them all to process
            await Task.Delay(500);

            //tear-down
            classicSerialPort.DataReceived -= ProcessData;
        }

        // the underlying OS provider only allows 255b messages to be sent on
        // the serial wire, so if we want to send a longer one, the `SerialPort`
        // class chunks it up
        async Task LongMessageTest()
        {
            string longMessage = @"Ticking away the moments that make up a dull day
Fritter and waste the hours in an offhand way.
Kicking around on a piece of ground in your home town
Waiting for someone or something to show you the way.
Tired of lying in the sunshine staying home to watch the rain.
You are young and life is long and there is time to kill today.
And then one day you find ten years have got behind you.
No one told you when to run, you missed the starting gun.
So you run and you run to catch up with the sun but it's sinking
Racing around to come up behind you again.
The sun is the same in a relative way but you're older,
Shorter of breath and one day closer to death.
Every year is getting shorter never seem to find the time.
Plans that either come to naught or half a page of scribbled lines
Hanging on in quiet desperation is the English way
The time is gone, the song is over,
Thought I'd something more to say.";

            Resolver.Log.Info("LongMessageTest");

            currentTestEncoding = Encoding.ASCII;

            classicSerialPort.DataReceived += ProcessData;

            await Task.Run(() =>
            {
                int written = classicSerialPort.Write(currentTestEncoding.GetBytes(longMessage));
                Resolver.Log.Info($"Wrote {written} bytes");
            });

            //weak ass Hack to wait for them all to process
            await Task.Delay(8000);

            //tear-down
            classicSerialPort.DataReceived -= ProcessData;
        }

        // anonymous method declaration so we can unwire later.
        void ProcessData(object sender, SerialDataReceivedEventArgs e)
        {
            Resolver.Log.Info("Serial Data Received");
            byte[] buffer = new byte[512];
            int bytesToRead = classicSerialPort.BytesToRead > buffer.Length
                                ? buffer.Length
                                : classicSerialPort.BytesToRead;
            while (true)
            {
                int readCount = classicSerialPort.Read(buffer, 0, bytesToRead);
                Console.Write(ParseToString(buffer, readCount, currentTestEncoding));
                // if we got all the data, break the while loop, otherwise, keep going.
                if (readCount < 512) { break; }
            }
            Console.Write("\n");
        }

        /// <summary>
        /// C# compiler doesn't allow Span<T> in async methods, so can't do this
        /// inline.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string ParseToString(byte[] buffer, int length, Encoding encoding)
        {
            Span<byte> actualData = buffer.AsSpan<byte>().Slice(0, length);
            return encoding.GetString(actualData);
        }
    }
}