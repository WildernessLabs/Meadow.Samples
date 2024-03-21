using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialMessagePort
{
    /// <summary>
    /// TODO: someone should really break these out into proper tests and whatnot.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        ISerialMessagePort serialPort;
        string delimiterString = "$$$";
        byte[] delimiterBytes;
        Encoding encoding = Encoding.UTF8;
        SerialPortName serialPortName;

        public override Task Initialize()
        {
            Resolver.Log.Info("Available serial ports:");
            foreach (var name in Device.PlatformOS.GetSerialPortNames())
            {
                Resolver.Log.Info($"  {name.FriendlyName}");
            }
            serialPortName = Device.PlatformOS.GetSerialPortName("COM1");

            Resolver.Log.Info("Get delimiter");
            // convert for later. 
            delimiterBytes = Encoding.ASCII.GetBytes(delimiterString);

            Resolver.Log.Info("SerialMessagePort_Test");
            Resolver.Log.Info($"Using '{serialPortName.FriendlyName}'...");
            Resolver.Log.Info($"delimiter:{delimiterString}");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            //await TestDoubleMessageWithSuffix();

            await TestSuffixDelimiter();
            //await TestPrefixDelimiter();

            //TestSuffixDelimeterAndBufferLengthForNulls();
        }

        /// <summary>
        /// Tests suffix/terminator delimited message reception.
        /// </summary>
        async Task TestSuffixDelimiter()
        {
            // TEST PARAM
            // whether or not to return the message with the tokens in it
            bool preseveDelimiter = true;

            // instantiate our serial port
            serialPort = Device.CreateSerialMessagePort(
                serialPortName, delimiterBytes, preseveDelimiter, baudRate: 115200);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            serialPort.Open();
            Resolver.Log.Info("\tOpened");

            // wire up message received handler
            serialPort.MessageReceived += SerialPort_MessageReceived;

            // write to the port.
            while (true)
            {
                foreach (var sentence in BuildVariableLengthTestSentences())
                {
                    //var dataToWrite = Encoding.ASCII.GetBytes($"{sentence}{DelimiterToken}");
                    var dataToWrite = Encoding.ASCII.GetBytes($"{sentence}").Concat(delimiterBytes).ToArray();
                    //var dataToWrite = Encoding.ASCII.GetBytes($"{sentence}") + delimiter;
                    var written = serialPort.Write(dataToWrite);
                    Resolver.Log.Info($"\nWrote {written} bytes");
                    // sleep
                    await Task.Delay(2000);
                }
            }
        }

        /// <summary>
        /// Test for https://github.com/WildernessLabs/Meadow_Issues/issues/102
        /// </summary>
        void TestSuffixDelimeterAndBufferLengthForNulls()
        {
            // instantiate our serial port
            serialPort = Device.CreateSerialMessagePort(
                serialPortName, Encoding.UTF8.GetBytes("\r\n"), false, baudRate: 115200);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            serialPort.Open();
            Resolver.Log.Info("\tOpened");

            // wire up message received handler
            serialPort.MessageReceived += (object sender, SerialMessageData e) =>
            {
                Resolver.Log.Info($"Message Lenght: {e.Message.Length}");
                if (e.Message.Length == 11)
                {
                    Resolver.Log.Info("Things are groovy.");
                }
                else
                {
                    Resolver.Log.Info("Things are not so groovy.");
                }
            };

            var dataToWrite = Encoding.ASCII.GetBytes($"TestMessage\r\n");
            var written = serialPort.Write(dataToWrite);
            Resolver.Log.Info($"\nWrote {written} bytes");
        }

        async Task TestDoubleMessageWithSuffix()
        {
            // TEST PARAM
            // whether or not to return the message with the tokens in it
            bool preseveDelimiter = false;

            // instantiate our serial port
            serialPort = Device.CreateSerialMessagePort(
                serialPortName, delimiterBytes, preseveDelimiter, baudRate: 115200);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            serialPort.Open();
            Resolver.Log.Info("\tOpened");

            // wire up message received handler
            serialPort.MessageReceived += SerialPort_MessageReceived;


            var dataToWrite = Encoding.ASCII.GetBytes($"{GetDoubleInOne()}").Concat(delimiterBytes).ToArray();
            var written = serialPort.Write(dataToWrite);
            Resolver.Log.Info($"\nWrote {written} bytes");
            // sleep
            await Task.Delay(2000);
        }

        async Task TestPrefixDelimiter()
        {
            // TEST PARAM
            // whether or not to return the message with the tokens in it
            bool preseveDelimiter = false;

            // instantiate our serial port
            serialPort = Device.CreateSerialMessagePort(
                serialPortName, delimiterBytes, preseveDelimiter, 27, baudRate: 115200);
            Resolver.Log.Info("\tCreated");

            // open the serial port
            serialPort.Open();
            Resolver.Log.Info("\tOpened");

            // wire up message received handler
            serialPort.MessageReceived += SerialPort_MessageReceived;

            // write to the port.
            while (true)
            {
                foreach (var sentence in BuildFixedLengthTestSentences())
                {
                    //var dataToWrite = Encoding.ASCII.GetBytes($"{sentence}{DelimiterToken}");
                    var dataToWrite = delimiterBytes.Concat(Encoding.ASCII.GetBytes($"{sentence}")).ToArray();
                    var written = serialPort.Write(dataToWrite);
                    Resolver.Log.Info($"\nWrote {written} bytes");
                    // sleep
                    await Task.Delay(2000);
                }
            }
        }


        private void SerialPort_MessageReceived(object sender, SerialMessageData e)
        {
            Resolver.Log.Info($"Msg recvd: {e.GetMessageString(Encoding.ASCII)}\n");
        }

        protected string[] BuildFixedLengthTestSentences()
        {
            return new string[]
            {
                "1234567890_abcdefghijklmnop",
                "quad erat demonstrandum foo",
                "eat your meat or no pudding",
                "another brick in the wall..",
                "life is better in the sun.."
            };
        }

        protected string GetDoubleInOne()
        {
            return $"TrickyDouble.{delimiterString}DoubleMessageTest";
        }

        protected string[] BuildVariableLengthTestSentences()
        {
            return new string[]
            {
                "Hello Meadow!",
                $"TrickyDouble.{delimiterString}DoubleMessageTest",
                "Ground control to Major Tom.",
                "Those evil-natured robots, they're programmed to destroy us",
                "Life, it seems, will fade away. Drifting further every day. Getting lost within myself, nothing matters, no one else.",
                "It's gonna be a bright, bright, sun-shiny day!",
                @"Ticking away the moments that make up a dull day
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
Thought I'd something more to say."
            };
        }
    }
}