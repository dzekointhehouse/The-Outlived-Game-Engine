namespace Spelkonstruktionsprojekt.ZEngine.Diagnostics
{
    public class SystemExecutionTime : ZLogger.IFile
    {
        private double ExecutionTime { get; }
        
        public SystemExecutionTime(double executionTime)
        {
            ExecutionTime = executionTime;
        }
        
        public override string ToString()
        {
            return ExecutionTime.ToString() + " ms";
        }
    }
}