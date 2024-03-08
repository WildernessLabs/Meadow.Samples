namespace MeadowWifi
{
    public class MeadowConfigFile
    {
        public Device Device { get; set; } = new Device();
        public Coprocessor Coprocessor { get; set; } = new Coprocessor();
        public Network Network { get; set; } = new Network();
    }
    public class Device
    {
        public string Name { get; set; } = "MeadowWifi";
    }
    public class Coprocessor
    {
        public bool AutomaticallyStartNetwork { get; set; } = true;
        public bool AutomaticallyReconnect { get; set; } = true;
        public int MaximumRetryCount { get; set; } = 7;
    }
    public class Network
    {
        public int GetNetworkTimeAtStartup { get; set; } = 1;

        public int NtpRefreshPeriod { get; set; } = 600;

        public string[] NtpServers { get; set; } =
        {
            "0.pool.ntp.org",
            "1.pool.ntp.org",
            "2.pool.ntp.org",
            "3.pool.ntp.org",
        };

        public string[] DnsServers { get; set; } =
        {
            "1.1.1.1",
            "8.8.8.8"
        };
    }
}