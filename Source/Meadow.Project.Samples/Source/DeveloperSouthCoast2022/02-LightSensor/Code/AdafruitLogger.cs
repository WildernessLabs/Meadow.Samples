using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LightSensor
{
    /// <summary>
    /// Provide the ability to send data to the Adafruit.IO service.
    /// </summary>
    public class AdafruitLogger
    {
        /// <summary>
        /// Key for the account being used.
        /// </summary>
        string _key;

        /// <summary>
        /// Account (user name) for the Adafruit.IO service.
        /// </summary>
        string _account;

        /// <summary>
        /// Create a new data logger class for the Adafruit IO service.
        /// </summary>
        /// <param name="account">Account (user name) for the Adafruit.IO service.</param>
        /// <param name="key">Key to the Adafruit.IO service.</param>
        public AdafruitLogger(string account, string key)
        {
            _key = key;
            _account = account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feed">Name of the feed to send the data to.</param>
        /// <param name="reading">Reading to send to the service.</param>
        public void Send(string feed, float reading)
        {
            Socket adafruit;

            IPHostEntry ipHostInfo = Dns.GetHostEntry("io.adafruit.com");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 80);

            adafruit = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            adafruit.SendTimeout = 3000;
            adafruit.ReceiveTimeout = 3000;
            adafruit.Connect(remoteEP);

            StringBuilder message = new StringBuilder();
            message.Append($"POST /api/v2/{_account}/feeds/{feed}/data HTTP/1.1\r\n");
            message.AppendFormat($"X-AIO-Key: {_key}\r\n");
            message.Append("Content-Type: application/json; charset=utf-8\r\n");

            StringBuilder readingText = new StringBuilder();
            readingText.Append("{\"value\":\"");
            readingText.AppendFormat($"{reading:F2}");
            readingText.Append("\"}");

            message.AppendFormat("Content-Length: {0}\r\n", readingText.Length);
            message.Append("Host: io.adafruit.com\r\n\r\n");
            message.Append(readingText);
            int bytesSent = adafruit.Send(Encoding.ASCII.GetBytes(message.ToString()));
            byte[] bytes = new byte[2048];
            int bytesReceived = adafruit.Receive(bytes);

            adafruit.Close();
        }
    }
}
