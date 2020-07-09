using System.Collections.Generic;

namespace PPchatLibrary
{
	class SimpleDictionary<Key, Value> : ISimpleDictionary<Key, Value>
	where Value : class
	{
		readonly IDictionary<Key, Value> Dictionary;

		public SimpleDictionary(IDictionary<Key, Value> dictionary)
			=> Dictionary = dictionary;
		public void Add(Key key, Value value)
			=> Dictionary.Add(key, value);

		public Value? GetValue(Key key)
		{
			Dictionary.TryGetValue(key, out var value);
			return value;
		}
	}

	class SimpleDictionary<Dictionary, Key, Value> : SimpleDictionary<Key, Value>
		where Value : class
		where Dictionary : IDictionary<Key, Value>, new()
	{
		public SimpleDictionary()
			: base(new Dictionary())
		{ }
	}
}
