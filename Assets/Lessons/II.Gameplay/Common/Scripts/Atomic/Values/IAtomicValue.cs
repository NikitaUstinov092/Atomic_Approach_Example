namespace Lessons.Gameplay
{
    public interface IAtomicValue<out T>
    {
        T Value { get; }
    }
}