using Meadow;
using System.Threading.Tasks;

namespace WinFormsMeadow;

public class Program
{
    public static async Task Main(string[] args)
    {
#if WINDOWS
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
#endif
        await MeadowOS.Start(args);
    }
}