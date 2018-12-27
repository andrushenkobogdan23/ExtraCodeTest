
namespace Shared.Common.Exceptions
{
    public class AggregateNotFoundException : AggregateBaseException
    {

        public AggregateNotFoundException(int id)
            : base("Aggregate with {0}='{1}' was not found.", id)
        {}
    }
}
