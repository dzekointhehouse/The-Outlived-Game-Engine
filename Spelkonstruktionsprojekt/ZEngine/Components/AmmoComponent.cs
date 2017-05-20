using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class AmmoComponent : IComponent
    {
        public int Amount { get; set; }
        public int SpareAmmoAmount { get; set; }
        public int OutOfAmmo { get; set; }

        public AmmoComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Amount = 0;
            SpareAmmoAmount = 0;
            OutOfAmmo = 0;
            return this;
        }
    }
}
