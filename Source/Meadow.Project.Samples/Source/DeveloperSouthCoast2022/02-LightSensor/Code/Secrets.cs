//
//  Enter your local specific details below and rename the file Secrets.cs
//
//  Note that Secrets.cs is excluded from git.
//
namespace LightSensor
{
    public class Secrets
    {
        /// <summary>
        /// Name of the WiFi network to use.
        /// </summary>
         public const string WIFI_NAME = "MarkS";
        //public const string WIFI_NAME = "WildernessLabs";

        /// <summary>
        /// Password for the WiFi network names in WIFI_NAME.
        /// </summary>
        public const string WIFI_PASSWORD = "rmRgecjjqeattS84Ryu01757709635";
        //public const string WIFI_PASSWORD = "Meadow30-10*!@26";

        /// <summary>
        /// Secret access key for Adafruit.io
        /// </summary>
        public const string APIO_KEY = "c4a9c3a50f2544379781de5ff95dda73";

        /// <summary>
        /// User name for Adafruit.io
        /// </summary>
        public const string APIO_USER_NAME = "nevyn";

        /// <summary>
        /// Feed name for the data in Adafruit.io
        /// </summary>
        public const string APIO_FEED_NAME = "my-feeds.lightsensor";

        /// <summary>
        /// IP address assigned to this board either manual or DHCP address.
        /// </summary>
        public const string IP_ADDRESS = "192.168.1.208";

        /// <summary>
        /// IP address of the computer serving web pages for the soak test.
        /// </summary>
        public const string SOAK_TEST_IP_ADDRESS = "192.168.4.1";

        /// <summary>
        /// Name of the web server on the local network.
        /// </summary>
        public const string LOCAL_WEB_SERVER_NAME = "TestServer";
    }
}
