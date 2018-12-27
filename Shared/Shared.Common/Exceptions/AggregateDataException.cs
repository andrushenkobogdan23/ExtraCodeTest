namespace Shared.Common.Exceptions
{
    public class AggregateDataException: AggregateBaseException
    {
        public AggregateDataException(string field, object value, string code) : 
            base("Aggregate with field {0} has wrong value '{1}'", field, value, code)
        { }

        public AggregateDataException(string field, object value) :
            this (field, value, null)
        { }
    }
}
