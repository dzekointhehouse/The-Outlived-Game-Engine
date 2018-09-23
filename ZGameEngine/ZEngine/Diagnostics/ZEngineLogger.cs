using System;
using System.Runtime.Remoting.Messaging;
using ZEngine.Managers;
using ZLogger;

namespace Spelkonstruktionsprojekt.ZEngine.Diagnostics
{
    public class ZEngineLogger
    {
        private Logger Logger { get; } = new Logger();
        private FilePrinter FilePrinter { get; } = FilePrinter.DebugPrinter();
        private AverageAnalyzer AverageAnalyzer { get; } = new AverageAnalyzer(new FileFactory());

        public void LogSystemTicks(string category, string label, double ticks)
        {
            Logger
                .GetOrCreate(category)
                .StoreFile(label, new SystemExecutionTicks(ticks));
        }

        public void PrintAverages()
        {
            FilePrinter.PrintLogs(AverageAnalyzer.AnalyzeAll(Logger.GetLogs().Values));
        }

        public void Print()
        {
            FilePrinter.PrintLogs(Logger.GetLogs());
        }
        
        
    }
}