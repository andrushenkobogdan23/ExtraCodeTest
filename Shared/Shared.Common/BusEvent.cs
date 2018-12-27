using Shared.Common.Interfaces;

namespace Shared.Common
{
    public class BusEvent : IBusEvent 
    {
        public BusEvent()
        { }

        //public short Version { get; internal protected set; }

    }

    public class TypedBusEvent<T> : BusEvent where T : struct
    {
        public TypedBusEvent() : base()
        { }

        public TypedBusEvent(T id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// domain model id
        /// </summary>
        public T Id { get; protected set; }
        //public short Version { get; internal protected set; }
    }
}
