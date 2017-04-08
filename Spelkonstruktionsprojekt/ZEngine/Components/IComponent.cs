namespace ZombieGame
{
    public interface IComponent
    {
        // All implementing classes must have a component name,
        // for accessability.
        string GetComponentName
        {
            get;
        }
    }
}