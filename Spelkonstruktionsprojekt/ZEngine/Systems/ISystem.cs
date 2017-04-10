using System.Security.Cryptography.X509Certificates;
using ZEngine.Wrappers;

namespace ZEngine.Managers
{
    public interface ISystem
    {
        void StartSystem(GameDependencies gd);
    }
}