namespace MeadowWifi;

public class WifiConfigFile
{
    public Credentials Credentials { get; set; }

    public WifiConfigFile(string ssid, string password)
    {
        Credentials = new Credentials()
        {
            Ssid = ssid,
            Password = password
        };
    }
}
public class Credentials
{
    public string Ssid { get; set; }
    public string Password { get; set; }
}