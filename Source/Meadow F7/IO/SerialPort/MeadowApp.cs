using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort;

/// <summary>
/// Loop Back testing of COMx with CRC calculated on the data to confirm no data lost or corrupted in transmission.
/// </summary>
/// <remarks>
/// To run these tests, create a loopback on the COM port to be tested and set portName
/// Optionally connect TTL serial RX to the loop back to monitor the data transmission.
/// </remarks>
/// <example>
///     F7FeatherV2, set portName="COM1" and connect D12 and D13.
///     F7CoreComputeV2, set portName="COM1" and connect D12 and D13.
/// </example>
public class MeadowApp : App<F7CoreComputeV2>
{
    private readonly string portName = "COM1";

    ISerialPort classicSerialPort;
    Encoding currentTestEncoding = Encoding.ASCII;
    private UInt32 crcRX = 0;
    private UInt32 crcTX = 0;

    public override Task Initialize()
    {
        Resolver.Log.Info("SerialPort Loopback testing.");

        Resolver.Log.Info("Available serial ports:");
        foreach (var name in Device.PlatformOS.GetSerialPortNames())
        {
            Resolver.Log.Info($"  {name.FriendlyName}");
        }

        Resolver.Log.Info($"Please loop TX to RX on the port being tested: {portName}");
        var serialPortName = Device.PlatformOS.GetSerialPortName(portName);

        if (serialPortName == null)
        {
            throw new InvalidOperationException($"Failed GetSerialPortName({portName}). Check comport name is valid and not in use.");
        }

        Resolver.Log.Info($"Using {serialPortName.FriendlyName}...");
        classicSerialPort = Device.CreateSerialPort(serialPortName, 115200);
        Resolver.Log.Info("\tCreated");
        // open the serial port
        classicSerialPort.Open();
        classicSerialPort.ClearReceiveBuffer();
        Resolver.Log.Info("\tOpened...");

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        await SimpleReadWriteTest(100, Encoding.ASCII);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        await SimpleReadWriteTest(50, Encoding.ASCII);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        for (int delay = 10; delay >= 2; delay--)
        {
            await SimpleReadWriteTest(delay, Encoding.ASCII);
            Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
            await Task.Delay(100); // this delay is to give Logging time to send trace
        }
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        for (int delay = 10; delay >= 2; delay--)
        {
            await SimpleReadWriteTest(delay, Encoding.Unicode);
            Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
            await Task.Delay(100); // this delay is to give Logging time to send trace
        }
        await SerialEventTest();
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        Resolver.Log.Info("LongMessageTest");
        await LongMessageTest(Encoding.ASCII);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        await LongMessageTest(Encoding.UTF8);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        await LongMessageTest(Encoding.Unicode);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace

        await LongMessageTest(Encoding.Unicode, 10);
        Resolver.Log.Info("----------------------------------------------------------------------------------------------------");
        await Task.Delay(200); // this delay is to give Logging time to send trace
    }

    /// <summary>
    /// Tests basic reading of serial in which the Write.Length == Read.Count
    /// </summary>
    async Task<bool> SimpleReadWriteTest(int delay, Encoding testEncoding, int count = 10)
    {
        //Note delay< +/ -2ms is invalid unless we have a way to know data has been sent. i.e.classicSerialPort.Flush();
        if (delay < 2)
        {
            Resolver.Log.Info($"SimpleReadWriteTest: delay = {delay}. Must be > 2ms");
            return false;
        }

        byte[] buffer = new byte[512];

        crcTX = crcRX = 0; // Reset to Zero for this test
        Resolver.Log.Info($"SimpleReadWriteTest: Start CRC = {crcRX:X08} delay={delay}");

        // run the test a few times.  
        for (int i = 1; i < 10; i++)
        {
            byte[] data = testEncoding.GetBytes($"{(count * i):D02} PRINT Hello Meadow! "); 
            crcTX = CRC.ComputeCRC(data, 0, data.Length, crcTX);
            var writeCount = classicSerialPort.Write(data);
            Resolver.Log.Info($"TX: CRC = {crcTX:X08} : {writeCount:D02} : Hex Data {ConvertToHexString(data)} Serial data: {ParseToString(data, data.Length, testEncoding)}");

            // leave time for TX of characters in ISR 
            if (delay > 0)
            {
                await Task.Delay(delay);
            }

            // empty it out
            var dataLength = classicSerialPort.BytesToRead;
            var readCount = classicSerialPort.Read(buffer, 0, dataLength);

            crcRX = CRC.ComputeCRC(buffer, 0, readCount, crcRX);
            Resolver.Log.Info($"RX: CRC = {crcRX:X08} : {readCount:D02} : Hex Data {ConvertToHexString(buffer, 0, readCount)} Serial data: {ParseToString(buffer, readCount, testEncoding)}");

        }

        Resolver.Log.Info($"SimpleReadWriteTest: End TxCRC = {crcTX:X08} RxCRC = {crcRX:X08} {(crcTX == crcRX ? "PASSED" : "FAILED")}");

        return crcTX == crcRX;
    }

    async Task<bool> SerialEventTest()
    {
        crcTX = crcRX = 0; // Reset to Zero for this test
        Resolver.Log.Info($"SerialEventTest: Start CRC = {crcRX:X08}");

        currentTestEncoding = Encoding.ASCII;
        classicSerialPort.DataReceived += ProcessData;

        // send some messages
        await Task.Run(() =>
        {
            Resolver.Log.Info("Sending 8 messages of profundity.");
            serialTestWrite("Ticking away the moments that make up a dull day,");
            serialTestWrite("fritter and waste the hours in an offhand way.");
            serialTestWrite("Kicking for someone or something to show you the way.");
            serialTestWrite("Tired of lying in the sunshine, staying home to watch the rain,");
            serialTestWrite("you are young and life is long and there is time to kill today.");
            serialTestWrite("And then one day you find ten years have got behind you,");
            serialTestWrite("No one told you when to run, you missed the starting gun.");
            return Task.CompletedTask;
        });

        //weak ass Hack to wait for them all to process
        // TODO: Implement classicSerialPort.Flush() to ensure TX buffer in NuttX is empty. Then remove delay. 
        await Task.Delay(5000);

        //tear-down
        classicSerialPort.DataReceived -= ProcessData;

        Resolver.Log.Info($"SerialEventTest: End TxCRC = {crcTX:X08} RxCRC = {crcRX:X08} {(crcTX == crcRX ? "PASSED" : "FAILED")}");
        
        return crcTX == crcRX;
    }

    int serialTestWrite(string text)
    {
        byte[] data = currentTestEncoding.GetBytes(text);
        crcTX = CRC.ComputeCRC(data, 0, data.Length, crcTX);
        return classicSerialPort.Write(data);
    }

    // the underlying OS provider only allows 511b messages to be sent on
    // the serial wire, so if we want to send a longer one, the `SerialPort`
    // class chunks it up
    async Task<bool> LongMessageTest(Encoding testEncoding, int count = 1)
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

        crcTX = crcRX = 0; // Reset to Zero for this test
        Resolver.Log.Info($"LongMessageTest: Start CRC = {crcRX:X08} Encoding = {testEncoding}");

        classicSerialPort.DataReceived += ProcessData;
        byte[] data = testEncoding.GetBytes(longMessage);

        for (int i = 0; i < count; i++)
        {
            crcTX = CRC.ComputeCRC(data, 0, data.Length, crcTX);
            Resolver.Log.Info($"LongMessageTest: Data  CRC = {crcTX:X08}");

            await Task.Run(() =>
            {
                int written = classicSerialPort.Write(data);
                Resolver.Log.Info($"Wrote {written} bytes");
            });
        }

        //weak ass Hack to wait for them all to process (Delay 1 full buffer)
        // TODO: implement classicSerialPort.Flush() or classicSerialPort.BytesToWrite()
        await Task.Delay((int)(1000.0 * (512.0/115200.0)));

        //tear-down
        classicSerialPort.DataReceived -= ProcessData;

        Resolver.Log.Info($"LongMessageTest: End TxCRC = {crcTX:X08} RxCRC = {crcRX:X08} {(crcTX == crcRX ? "PASSED" : "FAILED")}");

        return crcTX == crcRX;
    }



    // anonymous method declaration so we can unwire later.
    private void ProcessData(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[512];
        int bytesToRead = classicSerialPort.BytesToRead > buffer.Length
                            ? buffer.Length
                            : classicSerialPort.BytesToRead;
        while (true)
        {
            int readCount = classicSerialPort.Read(buffer, 0, bytesToRead);
            crcRX = CRC.ComputeCRC(buffer, 0, readCount, crcRX);
            Resolver.Log.Trace($"RX: CRC = {crcRX:X08} : {ParseToString(buffer, readCount, currentTestEncoding)} ");

            // if we got all the data, break the while loop, otherwise, keep going.
            if (readCount < 512) { break; }
        }
    }

    /// <summary>
    /// C# compiler doesn't allow Span<T> in async methods, so can't do this
    /// inline.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="length"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    private string ParseToString(byte[] buffer, int length, Encoding encoding)
    {
        char[] data = encoding.GetChars(buffer, 0, length);
        return new string(data);
    }

    private static string ConvertToHexString(byte[] data)
    {
        return ConvertToHexString(data, 0, data.Length);
    }

    private static string ConvertToHexString(byte[] data, int index, int length)
    {
        char[] hex = new char[(length - index) * 3];
        const string hexChars = "0123456789ABCDEF";

        for (int i = index; i < length; i++)
        {
            byte b = data[i];
            hex[i * 3] = hexChars[b >> 4];
            hex[i * 3 + 1] = hexChars[b & 0x0F];
            hex[i * 3 + 2] = ' ';
        }

        return new string(hex);
    }
}