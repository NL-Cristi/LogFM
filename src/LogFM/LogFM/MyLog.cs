using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogFM
{
    internal class MyLog
    {
        public static void FormatLogEntriesInFileByDateTime(string inputFilePath, string outputFilePath, bool writeSingleLine = false, string includeFilter = "", string excludeFilter = "")
        {
            try
            {
                var fileContent = File.ReadAllLines(inputFilePath);
                var logEntries = ParseLogEntries(fileContent);
                var sortedEntries = logEntries.OrderBy(entry => entry.Timestamp).ToList();

                using (var writer = new StreamWriter(outputFilePath))
                {
                    var includeFilterArray = string.IsNullOrEmpty(includeFilter) ? null : includeFilter.Split('|');
                    var excludeFilterArray = string.IsNullOrEmpty(excludeFilter) ? null : excludeFilter.Split('|');

                    foreach (var entry in sortedEntries)
                    {
                        bool includeEntry = includeFilterArray == null || includeFilterArray.Length == 0 || includeFilterArray.Any(indicator => entry.Content.Contains(indicator));
                        bool excludeEntry = excludeFilterArray != null && excludeFilterArray.Length > 0 && excludeFilterArray.Any(indicator => entry.Content.Contains(indicator));

                        // Determine if the entry should be written
                        bool shouldWrite = true;
                        if (includeFilterArray != null && includeFilterArray.Length > 0)
                            shouldWrite = shouldWrite && includeEntry;
                        if (excludeFilterArray != null && excludeFilterArray.Length > 0)
                            shouldWrite = shouldWrite && !excludeEntry;

                        if (shouldWrite)
                        {
                            var contentToWrite = writeSingleLine ? entry.Content.Replace("\n", " ") : entry.Content;
                            writer.WriteLine(contentToWrite);
                        }
                    }
                }

                Console.WriteLine($"Sorted log entries saved to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static List<MyLogEntry> ParseLogEntries(string[] lines)
        {
            var logEntries = new List<MyLogEntry>();
            var currentEntry = new MyLogEntry();
            var timestampRegex = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}");

            foreach (var line in lines)
            {
                if (timestampRegex.IsMatch(line))
                {
                    if (!string.IsNullOrEmpty(currentEntry.Content))
                    {
                        logEntries.Add(currentEntry);
                        currentEntry = new MyLogEntry();
                    }
                    currentEntry.Timestamp = DateTime.ParseExact(line.Substring(0, 23), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    currentEntry.Content = line;
                }
                else
                {
                    currentEntry.Content += "\n" + line;
                }
            }

            // Add the last entry if it's not empty
            if (!string.IsNullOrEmpty(currentEntry.Content))
            {
                logEntries.Add(currentEntry);
            }

            return logEntries;
        }
        
        public static async Task ProcessFilesInDirectory(Options opts)
        {
            var filesToProcess = Directory.GetFiles(opts.InputDir)
                .Where(file => Path.GetFileName(file).Contains(".log") && !Path.GetFileName(file).StartsWith("Formatted-"))
                .ToList();
            
            Parallel.ForEach(filesToProcess, (currentFile) =>
            {
                string inputFile = currentFile;
                string outputFile = opts.OutputFile ?? GenerateOutputFilePath(inputFile, opts.OutputDir);
                if (opts.Overwrite)
                {
                    FormatLogEntriesInFileByDateTime(inputFile, outputFile, opts.SingleLine, opts.IncludeFilter, opts.ExcludeFilter);
                }
                else
                {
                    Console.WriteLine("Error: File Exists, use Overwrite option to override!!!");
                }

         
            });
            if (opts.Merge)
            {
                MergeFiles(opts.OutputDir);

            }
        }
        private static void MergeFiles(string directoryPath)
        {
            var mergedFilePath = GenerateMergedLogFileName(directoryPath);
            using var mergedFile = new StreamWriter(mergedFilePath, false); // Overwriting if file exists
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith("Formatted-") && file != mergedFilePath) // Only include files that start with "Formatted-"
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        mergedFile.WriteLine(line);
                    }
                }
            }
        }

        static string GenerateOutputFilePath(string inputFile, string outputFolder)
        {
            string fileName = $"Formatted-{Path.GetFileName(inputFile)}";
            return outputFolder == null ? Path.Combine(Path.GetDirectoryName(inputFile), fileName)
                : Path.Combine(outputFolder, fileName);
        }
        static string GenerateMergedLogFileName(string inputFolder)
        {
            string fileName = "Merged-Formatted.log";
            return Path.Combine(inputFolder, fileName);
        }

    }
}
