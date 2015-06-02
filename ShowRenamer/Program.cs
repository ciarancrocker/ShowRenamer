using ShowRenamer.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShowRenamer
{
    class Program
    {
        private static readonly List<Regex> FileNameRegexes = new List<Regex>
        {
            new Regex("(?'title'.+)[sS](?'season'\\d+)[eE](?'episode'\\d+).*\\.(?'format'avi|mp4|mkv|mpeg|mpg)"),
            new Regex("(?'title'.+)(?'season'\\d+)[xX](?'episode'\\d+).*\\.(?'format'avi|mp4|mkv|mpeg|mpg)")
        };

        static int Main(string[] args)
        {
#if DEBUG
            // Initialise debug
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;
            Debug.Indent();
#endif

            Console.WriteLine("ShowRenamer 1.1.0, by Ciaran Crocker");
            Console.WriteLine("See my website for license information");
            DirectoryInfo workingDirectory;
            if (args.Length == 0)
            {
                Console.WriteLine("No directory provided, assuming '.'");
                workingDirectory = new DirectoryInfo(".");
            }
            else
            {
                try
                {
                    workingDirectory = new DirectoryInfo(args[0]);
                    Console.WriteLine($"Using {workingDirectory.FullName} as a working directory");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"An error occurred with the specified argument: {ex.Message}");
                    return byte.MaxValue;
                }
            }
            // NEW CODE
            IEnumerable<IFileNameProvider> fileNameProviders = PluginLoader.LoadFileNameProviders();
            Dictionary<FileInfo, FileNameContract> fileMappings = new Dictionary<FileInfo, FileNameContract>();
            foreach (FileInfo currentWorkingFile in workingDirectory.GetFiles())
            {
                Debug.WriteLine($"Processing file {currentWorkingFile.Name}");
                foreach(IFileNameProvider provider in fileNameProviders)
                {
                    Debug.WriteLine($"Trying provider {provider.GetType().Name}");
                    try
                    {
                        FileNameContract newFileName = provider.Recognize(currentWorkingFile.Name);
                        Debug.WriteLine($"Provider recognises file name.");
                        fileMappings[currentWorkingFile] = newFileName;
                        break;
                    }
                    catch(FileNameNotRecognisedException)
                    {
                        Debug.WriteLine($"Provider does't recognise filename.");
                        continue;
                    }
                }
            }
            if(fileMappings.Count == 0)
            {
                Console.WriteLine("No actions to be taken, press any key to exit.");
                Console.ReadKey();
                return 1;
            }
            WriteHeader("Filename change summary");
            foreach (KeyValuePair<FileInfo, FileNameContract> keyValuePair in fileMappings)
            {
                Console.WriteLine($"{keyValuePair.Key.Name} => {keyValuePair.Value.ToString()}");
            }
            Console.WriteLine("Press Y now to confirm changes, or any other key to exit.");
            if (Console.ReadKey().Key != ConsoleKey.Y)
                return 2;
            Console.WriteLine("Renaming files...");
            DateTime operationStartTime = DateTime.Now;
            foreach (KeyValuePair<FileInfo, FileNameContract> keyValuePair in fileMappings)
            {
                DateTime renameStartTime = DateTime.Now;
                File.Move(keyValuePair.Key.FullName, keyValuePair.Key.Directory.FullName + "\\" + keyValuePair.Value.ToString());
                TimeSpan renameTotalTime = DateTime.Now - renameStartTime;
                Console.WriteLine($"{keyValuePair.Key.Name} => {keyValuePair.Value.ToString()} ({renameTotalTime.TotalSeconds}s)");
            }
            TimeSpan operationTotalTime = DateTime.Now - operationStartTime;
            Console.WriteLine($"Renamed {fileMappings.Count} files in {operationTotalTime.TotalSeconds} seconds.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return 0;
        }

        private static void WriteHeader(string value)
        {
            int width = value.Length;
            if (value.Length > Console.WindowWidth)
                width = Console.WindowWidth;
            Console.WriteLine(value);
            WriteDivider(width);
            Console.WriteLine();
        }

        private static void WriteDivider(int width)
        {
            for (int index = 0; index < width; index++)
                Console.Write("-");
        }

        private static string SanitizeTitle(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.Replace('.', ' ').Replace('-', ' ').Replace('_', ' ').Trim());
        }
    }
}
