using System;

namespace Shared.Common
{
    public abstract class IntDomainModel : TypedDomainModel<int>
    {

    }

    public abstract class GuidDomainModel : TypedDomainModel<Guid>
    {

    }

    public abstract class TypedDomainModel<T> : SimpleDomainModel
    {
        public T Id { get;  set; }
    }

    public abstract class SimpleDomainModel
    {
    }
}
