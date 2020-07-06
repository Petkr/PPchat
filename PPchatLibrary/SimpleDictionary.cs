using System.Collections.Generic;

namespace PPchatLibrary
{
	class SimpleDictionary<Dictionary, Key, Value> : ISimpleDictionary<Key, Value>
		where Value : class
		where Dictionary : IDictionary<Key, Value>, new()
	{
		readonly IDictionary<Key, Value> dictionary;

		public SimpleDictionary()
			=> dictionary = new Dictionary();

		public void Add(Key key, Value value)
			=> dictionary.Add(key, value);

		public Value? GetValue(Key key)
		{
			dictionary.TryGetValue(key, out var value);
			return value;
		}
	}
}
