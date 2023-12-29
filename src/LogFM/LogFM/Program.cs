namespace LogFM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inFile = @"C:\Temp\SourceOld.log";
            var outFile = @"C:\Temp\FOrmatSourceOld.log";
            //timer to show how long next function takes
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            MyLog.FormatLogEntriesInFileByDateTime(inputFilePath: inFile, outputFilePath: outFile, excludeFilter: "", includeFilter: "", writeSingleLine: true);
            watch.Stop();
            //console write line the time elapsed, show ms for milliseconds s for seconds and m for minutes
            Console.WriteLine(Utils.GetFormattedElapsedTime(watch));
        }
    }
}
