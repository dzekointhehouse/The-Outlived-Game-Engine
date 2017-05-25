namespace ZEngine.Components
{
    class AIComponent : IComponent
    {
       // public Vector2 Target;

        public bool Wander { get; set; }

        public float FollowDistance { get; set; }

        public double TimeOfLastWallCollision { get; set; } = 0;
        
        public AIComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Wander = false;
            FollowDistance = 250;
            return this;
        }
    }
}
    