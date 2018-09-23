using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.Input
{
    public class GamePadComponent : IComponent
    {
        public int GamePadPlayerIndex;
        
        public IComponent Reset()
        {
            GamePadPlayerIndex = 0;
            return this;
        }
    }
}