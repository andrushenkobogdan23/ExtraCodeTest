using System;

namespace Shared.Common.Exceptions
{
    public class EventDeserializationException : Exception
    {
        public readonly string EventTypeName;
        public readonly string Metadata;

        public EventDeserializationException(string eventTypeName, string metadata)
            : base(string.Format("Could not deserialize {0} as an Event (metadata: {1})", eventTypeName, metadata))
        {
            EventTypeName = eventTypeName;
            Metadata = metadata;
        }
    }
}
