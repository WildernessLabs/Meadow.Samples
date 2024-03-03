using Meadow;
using Meadow.Devices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            Resolver.Log.Info("Meadow File System Tests");
            // list out the named directories available at MeadowOS.FileSystem.[x]
            EnumerateNamedDirectories();

            // get the size of the `app.exe` file.
            FileStatus(Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, "App.exe"));

            // create a `hello.txt` file in the `/Temp` directory
            CreateFile(MeadowOS.FileSystem.TempDirectory, "hello.txt");

            // check on that file.
            FileStatus(Path.Combine(MeadowOS.FileSystem.TempDirectory, "hello.txt"));

            //Tree("/", true);
            // write out a tree of all files in the user file system root
            Tree(MeadowOS.FileSystem.UserFileSystemRoot, true);

            Resolver.Log.Info("Testing complete");

            return Task.CompletedTask;
        }

        void EnumerateNamedDirectories()
        {
            Resolver.Log.Info("The following named directories are available:");
            Resolver.Log.Info($"\t MeadowOS.FileSystem.UserFileSystemRoot: {MeadowOS.FileSystem.UserFileSystemRoot}");
            Resolver.Log.Info($"\t MeadowOS.FileSystem.CacheDirectory: {MeadowOS.FileSystem.CacheDirectory}");
            Resolver.Log.Info($"\t MeadowOS.FileSystem.DataDirectory: {MeadowOS.FileSystem.DataDirectory}");
            Resolver.Log.Info($"\t MeadowOS.FileSystem.DocumentsDirectory: {MeadowOS.FileSystem.DocumentsDirectory}");
            Resolver.Log.Info($"\t MeadowOS.FileSystem.TempDirectory: {MeadowOS.FileSystem.TempDirectory}");
        }

        void CreateFile(string path, string filename)
        {
            Resolver.Log.Info($"Creating '{path}/{filename}'...");

            if (!Directory.Exists(path))
            {
                Resolver.Log.Info("Directory doesn't exist, creating.");
                Directory.CreateDirectory(path);
            }

            try
            {
                using (var fs = File.CreateText(Path.Combine(path, filename)))
                {
                    fs.WriteLine("Hello Meadow File!");
                }
            }
            catch (Exception ex)
            {
                Resolver.Log.Info(ex.Message);
            }
        }

        void FileStatus(string path)
        {
            Console.Write($"FileStatus() File: {Path.GetFileName(path)} ");
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    Resolver.Log.Info($"Size: {stream.Length,-8}");
                }
            }
            catch (Exception ex)
            {
                Resolver.Log.Info(ex.Message);
            }
        }

        void Tree(string root, bool showSize = false)
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

        void DirectoryListTest(string path)
        {
            Resolver.Log.Info($"Enumerating path '{path}'");

            var dirs = Directory.GetDirectories(path);
            Resolver.Log.Info($" Found {dirs.Length} Directories {((dirs.Length > 0) ? ":" : string.Empty)}");
            foreach (var d in dirs)
            {
                Resolver.Log.Info($"   {d}");
            }
            var files = Directory.GetFiles(path);
            Resolver.Log.Info($" Found {files.Length} Files {((files.Length > 0) ? ":" : string.Empty)}");
            foreach (var f in files)
            {
                Resolver.Log.Info($"   {f}");
            }
        }

        void DirectoryListTest2()
        {
            Resolver.Log.Info("Enumerating logical drives...");

            var drives = Directory.GetLogicalDrives();

            Resolver.Log.Info($" Found {drives.Length} logical drives");

            foreach (var d in drives)
            {
                Resolver.Log.Info($"  DRIVE '{d}'");

                ShowFolder(d, 3);

                void ShowFolder(string path, int indent)
                {
                    var name = Path.GetDirectoryName(path);
                    name = string.IsNullOrEmpty(name) ? "/" : name;
                    Resolver.Log.Info($"{new string(' ', indent)}+ {name}");

                    foreach (var fse in Directory.GetFileSystemEntries(path))
                    {
                        Resolver.Log.Info($"{new string(' ', indent + 3)}fse {fse}");
                    }

                    foreach (var dir in Directory.GetDirectories(d))
                    {
                        ShowFolder(dir, indent + 1);
                    }

                    foreach (var f in Directory.GetFiles(path))
                    {
                        Resolver.Log.Info($"{new string(' ', indent + 3)}f{f}");

                        var fi = new FileInfo(f);
                        if (fi.Exists)
                        {
                            Resolver.Log.Info($"{new string(' ', indent + 4)} Exists as file");
                        }
                        var di = new DirectoryInfo(f);
                        if (fi.Exists)
                        {
                            Resolver.Log.Info($"{new string(' ', indent + 4)} Exists as directory");
                        }
                    }
                }
            }

            Resolver.Log.Info("Opening file as a dir...");
            foreach (var f in Directory.GetFiles("/meadow0"))
            {
                Resolver.Log.Info($"f {f}");
            }
        }
    }
}