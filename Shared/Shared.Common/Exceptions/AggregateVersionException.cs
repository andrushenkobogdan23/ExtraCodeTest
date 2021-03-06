﻿using System;

namespace Shared.Common.Exceptions
{
    public class AggregateVersionException : Exception
    {
        public readonly int Id;
        public readonly Type Type;
        public readonly int AggregateVersion;
        public readonly int RequestedVersion;

        public AggregateVersionException(int id, Type type, int aggregateVersion, int requestedVersion)
            : base(string.Format("Requested version {2} of aggregate '{0}' (type {1}) - aggregate version is {3}", id, type.Name, requestedVersion, aggregateVersion))
        {
            Id = id;
            Type = type;
            AggregateVersion = aggregateVersion;
            RequestedVersion = requestedVersion;
        }
    }
}
