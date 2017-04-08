
namespace ZEngine.Components
{
    public interface Component
    {
        // All implementing classes must have a component name,
        // for accessability.
        string GetComponentName
        {
            get;
        }
    }
}