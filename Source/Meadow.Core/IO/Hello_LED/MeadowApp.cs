using Meadow;
using Meadow.Devices;
using System.IO;
using System.Threading.Tasks;

namespace Hello_LED
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Creating Directories and files...");

            var f = 0;

            var di = new DirectoryInfo(MeadowOS.FileSystem.DataDirectory);
            if (!di.Exists)
            {
                di.Create();
            }

            for (; f < 5; f++)
            {
                var fi = new FileInfo(Path.Combine(MeadowOS.FileSystem.DataDirectory, $"File_{f++}.txt"));
                if (!fi.Exists)
                {
                    fi.Create().Close();
                    File.WriteAllText(fi.FullName, "TEST FILE CONTENTS");
                }
            }

            di = new DirectoryInfo(MeadowOS.FileSystem.DocumentsDirectory);
            if (!di.Exists)
            {
                di.Create();
            }
            for (; f < 8; f++)
            {
                var fi = new FileInfo(Path.Combine(MeadowOS.FileSystem.DataDirectory, $"File_{f++}.xml"));
                if (!fi.Exists)
                {
                    fi.Create().Close();
                    File.WriteAllText(fi.FullName, "<TEST FILE CONTENTS />");
                }
            }


            return Task.CompletedTask;
        }
    }
}