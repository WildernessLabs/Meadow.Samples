using Meadow;
using System.Threading.Tasks;

namespace I2C_Sensors;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}