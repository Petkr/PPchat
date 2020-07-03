using System;
using System.Collections.Generic;
using System.Linq;

namespace PPchatLibrary
{
	abstract class BasicInfo<Key, Info, TypeToScan> : IInfo<Key, Info>
		where Info : class
		where Key : notnull
	{
		protected static IEnumerable<Type> GetImplementedInterfaces<Implementation>(Type genericInterfaceDefinitionType)
			=> typeof(Implementation).GetInterfaces()
			.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceDefinitionType)
			.Select(x => x.GetGenericArguments()[0]);

		readonly IDictionary<Key, Info> Map;

		public BasicInfo(Type genericInterfaceDefinitionType)
		{
			Map = new Dictionary<Key, Info>();

			foreach (var t in GetImplementedInterfaces<TypeToScan>(genericInterfaceDefinitionType))
				Handle(t);
		}

		protected abstract void Handle(Type type);

		protected void AddInfo(Key from, Info to)
			=> Map.Add(from, to);

		public Info? GetInfo(Key from)
		{
			Map.TryGetValue(from, out var result);
			return result;
		}
	}
}
