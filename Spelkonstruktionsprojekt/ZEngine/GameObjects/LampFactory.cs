using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Spelkonstruktionsprojekt.ZEngine.GameObjects
{
    public class LampFactory
    {
        public uint FlickeringLamp()
        {
            var lamp = new EntityBuilder()
                .SetRendering(15, 15);
            return 0;
        }
    }
}