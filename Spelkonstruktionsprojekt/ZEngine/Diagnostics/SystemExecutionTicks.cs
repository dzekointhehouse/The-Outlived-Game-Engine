using ZLogger;

namespace Spelkonstruktionsprojekt.ZEngine.Diagnostics
{
    public class SystemExecutionTicks : ZLogger.IFile, INumerical
    {
        private double ExecutionTicks { get; }

        public SystemExecutionTicks(double executionTicksTicks)
        {
            ExecutionTicks = executionTicksTicks;
        }

        public override string ToString()
        {
            return ExecutionTicks.ToString() + " ticks";
        }

        public double Value()
        {
            return ExecutionTicks;
        }
    }
}