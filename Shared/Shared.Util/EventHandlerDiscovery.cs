using Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Common.Util
{
    public class EventHandlerDiscovery
    {
        public Dictionary<Type, SimpleDomainModel> Handlers
        {
            get; private set;
        }

        public EventHandlerDiscovery()
        {
            Handlers = new Dictionary<Type, SimpleDomainModel>();
        }

        public EventHandlerDiscovery Scan(SimpleDomainModel obj)
        {
            var handlerInterface = typeof(IHandle<>);
            var aggType = obj.GetType();

            var interfaces = aggType.GetInterfaces();

            var instances = from i in aggType.GetInterfaces()
                            where (i.IsGenericType && handlerInterface.IsAssignableFrom(i.GetGenericTypeDefinition()))
                            select i.GenericTypeArguments[0];

            foreach (var i in instances)
            {
                Handlers.Add(i, obj);
            }

            return this;
        }
    }
}
