
namespace ZEngine.Components
{
    public interface IComponent
    {
        // All implementing classes must have a component name,
        // for accessability.
        // Ofcourse august likes to have a string instead of using type of.
        string GetComponentName
        {
            get;
        }
    }
}