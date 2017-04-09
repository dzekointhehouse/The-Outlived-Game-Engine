namespace ZEngine.Managers
{
    public interface ISystem
    {
        ISystem Start();

        ISystem Stop();
    }
}