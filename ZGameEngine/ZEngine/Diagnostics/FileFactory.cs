using System.Runtime.InteropServices;
using ZLogger;

namespace Spelkonstruktionsprojekt.ZEngine.Diagnostics
{
    public class FileFactory : INumericalFactory
    {
        public IFile Create(double value)
        {
            return new SystemExecutionTicks(value);
        }
    }
}