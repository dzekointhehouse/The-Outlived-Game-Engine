using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class AmmoComponent : IComponent
    {
        public int Amount { get; set; }

        public int SpareAmmoAmount { get; set; }
    }
}
