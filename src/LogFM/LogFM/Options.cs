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

        [Option(shortName: 't', longName: "output-dir", Required = false,
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



        public bool InputIsValid()
        {
            return !string.IsNullOrWhiteSpace(InputFile) || !string.IsNullOrWhiteSpace(InputDir);
        }
    }
}