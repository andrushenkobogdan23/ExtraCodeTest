namespace Shared.Common.Interfaces
{
    public interface IHandle<in T> where T : BusEvent
    {
    }
}
