using System.Security.Cryptography.X509Certificates;

namespace ZEngine.Managers
{
    public interface ISystem
    {
        ISystem Start();

        ISystem Stop();

        //void SystemProcess();
    }
}