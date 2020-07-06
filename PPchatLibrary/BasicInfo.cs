using System;
using System.Collections.Generic;
using System.Linq;

namespace PPchatLibrary
{
	abstract class BasicInfo<Key, Value, TypeToScan> : ISimpleDictionary<Key, Value>
		where Value : class
		where Key : notnull
	{
		protected static IEnumerable<Type> GetImplementedInterfaces<Implementation>(Type genericInterfaceDefinitionType)
			=> typeof(Implementation).GetInterfaces()
			.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceDefinitionType)
			.Select(x => x.GetGenericArguments()[0]);

		readonly IDictionary<Key, Value> Map;

		public BasicInfo(Type genericInterfaceDefinitionType)
		{
			Map = new Dictionary<Key, Value>();

			foreach (var t in GetImplementedInterfaces<TypeToScan>(genericInterfaceDefinitionType))
				Handle(t);
		}

		protected abstract void Handle(Type type);

		public void Add(Key from, Value to)
			=> Map.Add(from, to);

		public Value? GetValue(Key from)
		{
			Map.TryGetValue(from, out var result);
			return result;
		}
	}
}
