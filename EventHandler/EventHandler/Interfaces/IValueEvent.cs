namespace EventHandler.Interfaces
{
    public interface IValueEvent<T> : IEvent
    {
        T Value { get; set; }
    }
}