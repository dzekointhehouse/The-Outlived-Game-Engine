using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class CageComponent : IComponent
    {
        public int CageId { get; set; }

        public IComponent Reset()
        {
            CageId = -1;
            return this;
        }
    }
}
