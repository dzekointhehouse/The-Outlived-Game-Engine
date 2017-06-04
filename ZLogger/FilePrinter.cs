using System;
using System.Collections.Generic;

namespace ZLogger
{
    public class FilePrinter
    {
        private IPrinter Printer { get; }

        public static FilePrinter DebugPrinter()
        {
            return new FilePrinter(new Printer());    
        }
        
        public FilePrinter(IPrinter printer)
        {
            Printer = printer;
        }

        public void PrintLogs(Dictionary<string, Cabinet> logs)
        {
            foreach (var cabinet in logs)
            {
                PrintCabinetContents(cabinet.Value);
            }
        }
        
        public void PrintCabinetContents(Cabinet cabinet)
        {
            Printer.Header(cabinet.Label);
            foreach (var folder in cabinet.GetAllFolders())
            {
                PrintFolder(folder.Key, folder.Value);
            }
        }
        
        public void PrintFolder(string type, List<IFile> files)
        {
            Printer.Header(type);
            foreach (var file in files)
            {
                PrintFile(type, file);
            }
        }
        
        public void PrintFile(string type, IFile file)
        {
            Printer.LabeledLine(type, file.ToString());
        }

    }
}