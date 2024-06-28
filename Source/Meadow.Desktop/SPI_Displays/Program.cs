using Meadow;
using System.Threading.Tasks;

namespace SPI_Displays;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}