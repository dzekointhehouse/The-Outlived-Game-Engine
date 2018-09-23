namespace ZEngine.Components
{
    public class InertiaDampeningComponent : IComponent
    {
        public int StabilisingSpeed;

        public InertiaDampeningComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            StabilisingSpeed = 1000;
            return this;
        }
    }
}