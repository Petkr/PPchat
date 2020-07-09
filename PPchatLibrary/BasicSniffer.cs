using System;
using System.Collections.Generic;
using System.Linq;

namespace PPchatLibrary
{
	abstract class BasicSniffer<Key, Value, TypeToScan> : SimpleDictionary<Key, Value>
		where Value : class
		where Key : notnull
	{
		protected static IEnumerable<Type> GetImplementedInterfaces<Implementation>(Type genericInterfaceDefinitionType)
			=> typeof(Implementation).GetInterfaces()
			.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceDefinitionType)
			.Select(x => x.GetGenericArguments()[0]);

		public BasicSniffer(IDictionary<Key, Value> map, Type genericInterfaceDefinitionType)
			: base(map)
		{
			foreach (var t in GetImplementedInterfaces<TypeToScan>(genericInterfaceDefinitionType))
				Handle(t);
		}

		protected abstract void Handle(Type type);
	}
}
