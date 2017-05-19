using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class AmmoComponent : IComponent
    {
        public int Amount { get; set; } = 0;
        public int SpareAmmoAmount { get; set; } = 0;
        public int OutOfAmmo { get; set; } = 0;
    }
}
