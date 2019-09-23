using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {

            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = "c:\\csv\\";
                //watcher.Path = args[1];];

                watcher.IncludeSubdirectories = true;
                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.csv";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }
        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        // Specify what is done when a file is changed, created, or deleted.
        {
            Console.WriteLine("OnChanged");

            try
            {
                string search_text = "DataLog";
                string old;
                string n = "";
                //string FileName = "C:\\Log_20180319140301_20190706180348.csv";
                string FileName = e.FullPath;
                StreamReader sr = File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                File.WriteAllText(FileName, n);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.ToString()}");
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e) =>
            // Specify what is done when a file is renamed.
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        private static void OnDeleted(object source, RenamedEventArgs e) =>
            // Specify what is done when a file is renamed.
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");


        static void cutTxt()
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader("file.txt"))
            using (var sw = new StreamWriter(tempFile))
            {
                int id = 1;
                string line;

                sw.WriteLine(sr.ReadLine());
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "DataLog" || id != 1)
                        sw.WriteLine(line);
                }
            }

            File.Delete("file.txt");
            File.Move(tempFile, "file.txt");
        }
    }
}
