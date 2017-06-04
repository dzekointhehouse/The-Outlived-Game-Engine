using System;
using System.Diagnostics;

namespace ZLogger
{
    public class Printer : IPrinter
    {
        public void LabeledLine(string label, string line)
        {
            PrintLine(label + ":\t" + line);
        }

        public void Header(string header)
        {
            PrintLine("\n\t\t" + header);
        }
        
        public void PrintLine(string line)
        {
            Debug.WriteLine(line);
        }

        public void Print(string text)
        {
            Debug.Write(text);
        }
    }
}