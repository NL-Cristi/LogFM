using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CommandLine;


namespace LogFM
{
    public class Options
    {
        [Option(shortName: 'i', longName: "input-file", Required = false, HelpText = "Input logFile path.")]
        public string? InputFile { get; set; }

        [Option(shortName: 'o', longName: "output-file", Required = false, HelpText = "Output file path.")]
        public string? OutputFile { get; set; }

        [Option(shortName: 's', longName: "input-dir", Required = false, HelpText = "Source Directory where logs are.")]
        public string? InputDir { get; set; }

        [Option(shortName: 'd', longName: "output-dir", Required = false,
            HelpText = "Target Directory where formatted files will be saved.")]
        public string? OutputDir { get; set; }

        [Option(shortName: 'l', longName: "single-line", Required = false, HelpText = "Each LogEntry on one Line",
            Default = false)]
        public bool SingleLine { get; set; }

        [Option(shortName: 'm', longName: "merge", Required = false,
            HelpText = "Merge all files from inputDir to one MERGED LOG.", Default = false)]
        public bool Merge { get; set; }

        [Option(shortName: 'w', longName: "over-write", Required = false,
            HelpText = "Overwrite the output file if it exists.", Default = false)]
        public bool Overwrite { get; set; }

        [Option(shortName: 'f', longName: "include-filter", Required = false,
            HelpText = "Filter to include specific log entries.", Default = "")]
        public string IncludeFilter { get; set; } = "";

        [Option(shortName: 'e', longName: "exclude-filter", Required = false,
            HelpText = "Filter to exclude specific log entries.", Default = "")]
        public string ExcludeFilter { get; set; } = "";

        [Option(shortName: 'v', longName: "verbose", Required = false, HelpText = "Set output to verbose.")]
        public bool Verbose { get; set; }

        public bool ValidateOptions()
        {
            // Check if input is provided
            if (string.IsNullOrWhiteSpace(InputFile) && string.IsNullOrWhiteSpace(InputDir))
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Error: Either an input file or an input directory must be provided.");
                Console.ResetColor();
                Console.WriteLine(@"-i, --input-file        Input logFile path.
  -o, --output-file       Output file path.
  -s, --input-dir         Source Directory where logs are.
  -d, --output-dir        Target Directory where formatted files will be saved.
  -l, --single-line       (Default: false) Each LogEntry on one Line
  -m, --merge             (Default: false) Merge all files from inputDir to one MERGED LOG.
  -w, --over-write        (Default: false) Overwrite the output file if it exists.
  -f, --include-filter    (Default: ) Filter to include specific log entries.
  -e, --exclude-filter    (Default: ) Filter to exclude specific log entries.
  -v, --verbose           Set output to verbose.
  --help                  Display this help screen.
  --version               Display version information.");
                return false;
            }

            // Check if the input file or directory exists
            if (!string.IsNullOrWhiteSpace(InputFile) && !File.Exists(InputFile))
            {
                Console.WriteLine($"ErrorInput: The input file '{InputFile}' does not exist.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(InputDir) && !Directory.Exists(InputDir))
            {
                Console.WriteLine($"Error: The input directory '{InputDir}' does not exist.");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(OutputDir) && !Directory.Exists(OutputDir))
            {
                Console.WriteLine($"Error: The input directory '{OutputDir}' does not exist.");
                return false;
            }

            // Set default output file if neither output file nor OutputDir is specified
            if (string.IsNullOrWhiteSpace(OutputFile) && string.IsNullOrWhiteSpace(OutputDir) && string.IsNullOrWhiteSpace(InputDir))
            {
                OutputFile = GetfullFormattedFilePath(InputFile);

                // Console.WriteLine($"Ouptut is: {OutputFile}");

            }

            // If merge option is selected, ensure OutputDir is specified
            if (Merge && string.IsNullOrWhiteSpace(OutputDir) && !string.IsNullOrWhiteSpace(InputDir))
            {
                OutputDir = InputDir;
                return true;
            }

            return true;
        }
        static string GetfullFormattedFilePath(string fullPath)
        {
            string directory = Path.GetDirectoryName(fullPath);
            string fileName = $"Formatted-{Path.GetFileName(fullPath)}";
            string fullFormattedFilePath = Path.Combine(directory, fileName);

            return fullFormattedFilePath;
        }
    }
}