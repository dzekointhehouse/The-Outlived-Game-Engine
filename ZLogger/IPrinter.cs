namespace ZLogger
{
    public interface IPrinter
    {
        void PrintLine(string line);

        void Print(string text);

        void LabeledLine(string label, string line);

        void Header(string header);
    }
}