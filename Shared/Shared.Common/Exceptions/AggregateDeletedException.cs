
namespace Shared.Common.Exceptions
{
    public class AggregateDeletedException<T> : AggregateBaseException
    {
        public AggregateDeletedException(int id) :
            base("Aggregate with field " + "{0} has wrong value'{1}'", id)
        { }
    }
}
