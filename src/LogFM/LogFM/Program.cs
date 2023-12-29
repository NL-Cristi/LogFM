using CommandLine;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogFM
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Parse the command line arguments
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async opts => await RunWithOptions(opts))
                .WithNotParsed(HandleParseError);
        }

        static async Task RunWithOptions(Options opts)
        {
            // Validate the options
            if (!opts.ValidateOptions())
            {
                Console.WriteLine("Validation failed. Exiting.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(opts.InputFile)&& string.IsNullOrWhiteSpace(opts.InputDir))
            {
                if (opts.Overwrite)
                {
                    MyLog.FormatLogEntriesInFileByDateTime(opts.InputFile, opts.OutputFile, opts.SingleLine, opts.IncludeFilter, opts.ExcludeFilter);
                }
                else
                {
                    Console.WriteLine("Error: File Exists, use Overwrite option to override!!!");
                }

            }
            else if (!string.IsNullOrWhiteSpace(opts.InputDir)&& string.IsNullOrWhiteSpace(opts.InputFile))
            {
               await MyLog.ProcessFilesInDirectory(opts);
            }
            else
            {
                Console.WriteLine("No valid input provided.");
            }
        }

        static void HandleParseError(IEnumerable<CommandLine.Error> errs)
        {
            // Handle errors related to argument parsing
            // ... Error handling logic ...
        }
    }
}