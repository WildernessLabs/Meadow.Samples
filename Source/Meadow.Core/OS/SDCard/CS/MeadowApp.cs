using Meadow;
using Meadow.Devices;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDCard
{
    public class MeadowApp : App<F7CoreComputeV2>
    {
        public override Task Run()
        {
            Device.PlatformOS.FileSystem.ExternalStorageEvent += PlatformOS_ExternalStorageEvent;

            var drive = Device.PlatformOS.FileSystem.Drives.FirstOrDefault(d => d is IExternalStorage);

            if (drive == null)
            {
                Resolver.Log.Warn($"SD card is not detected");
            }
            else
            {
                Resolver.Log.Info($"SD card is mounted at: {drive.Name}");

                Tree(drive.Name, true);
            }

            Resolver.Log.Info("Waiting for storage events...");

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private void PlatformOS_ExternalStorageEvent(IExternalStorage storage, ExternalStorageState state)
        {
            Resolver.Log.Info($"Storage Event: {storage.Name} is {state}");

            if (state == ExternalStorageState.Inserted)
            {
                var random = new Random();

                var name = $"test_{random.Next(32768)}.txt";

                using (var file = File.CreateText(Path.Combine(storage.Name, name)))
                {
                    file.Write("Hello Meadow!");
                }

                Resolver.Log.Info($"Created {name}");

                Tree(storage.Name, true);
            }
        }

        private void Tree(string root, bool showSize = false)
        {
            var fileCount = 0;
            var folderCount = 0;

            ShowFolder(root, 0);
            Resolver.Log.Info(string.Empty);
            Resolver.Log.Info($"{folderCount} directories");
            Resolver.Log.Info($"{fileCount} files");

            void ShowFolder(string folder, int depth, bool last = false)
            {
                string[] files = null;

                try
                {
                    files = Directory.GetFiles(folder);
                    Resolver.Log.Info($"{GetPrefix(depth, last && files.Length == 0)}{Path.GetFileName(folder)}");
                }
                catch
                {
                    Resolver.Log.Info($"{GetPrefix(depth, last)}{Path.GetFileName(folder)}");
                    Resolver.Log.Info($"{GetPrefix(depth + 1, last)}<cannot list files>");
                }
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var prefix = GetPrefix(depth + 1, last);
                        if (showSize)
                        {
                            FileInfo fi = null;
                            try
                            {
                                fi = new FileInfo(file);
                                prefix += $"[{fi.Length,8}]  ";

                            }
                            catch
                            {
                                prefix += $"[   error]  ";
                            }
                        }
                        Resolver.Log.Info($"{prefix}{Path.GetFileName(file)}");
                        fileCount++;
                    }
                }

                string[] dirs = null;
                try
                {
                    dirs = Directory.GetDirectories(folder);
                }
                catch
                {
                    if (files == null || files.Length == 0)
                    {
                        Resolver.Log.Info($"{GetPrefix(depth + 1, last)}<cannot list sub-directories>");
                    }
                    else
                    {
                        Resolver.Log.Info($"{GetPrefix(depth + 1)}<cannot list sub-directories>");
                    }
                }
                if (dirs != null)
                {
                    for (var i = 0; i < dirs.Length; i++)
                    {
                        ShowFolder(dirs[i], depth + 1, i == dirs.Length - 1);
                        folderCount++;
                    }
                }

                string GetPrefix(int d, bool isLast = false)
                {
                    var p = string.Empty;

                    for (var i = 0; i < d; i++)
                    {
                        if (i == d - 1)
                        {
                            p += "+--";
                        }
                        else if (isLast && i == d - 2)
                        {
                            p += "   ";
                        }
                        else
                        {
                            p += "|  ";
                        }
                    }

                    return p;
                }
            }
        }
    }
}