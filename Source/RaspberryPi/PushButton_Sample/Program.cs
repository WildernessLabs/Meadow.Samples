using Meadow;
using System.Threading.Tasks;

namespace PushButton_Sample;

public class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}