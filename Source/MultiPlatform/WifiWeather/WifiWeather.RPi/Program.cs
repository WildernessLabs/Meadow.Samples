using Meadow;
using System.Threading.Tasks;

namespace WifiWeather.RPi;

public class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}