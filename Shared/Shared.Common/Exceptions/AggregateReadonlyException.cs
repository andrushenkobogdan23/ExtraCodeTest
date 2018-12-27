
namespace Shared.Common.Exceptions
{
    public class AggregateReadonlyException : AggregateBaseException
    {

        public AggregateReadonlyException(int id)
            : base("Aggregate with {0}='{1}' is readonly", id)
        {}

    }
}
