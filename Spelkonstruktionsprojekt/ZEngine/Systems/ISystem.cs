using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public interface ISystem
    {
        void Start();
        void Stop();
    }
}
