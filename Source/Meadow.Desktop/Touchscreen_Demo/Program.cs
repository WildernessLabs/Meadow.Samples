using Meadow;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

public class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}