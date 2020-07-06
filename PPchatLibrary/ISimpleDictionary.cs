namespace PPchatLibrary
{
	interface ISimpleDictionary<Key, Value> : ISimpleReadonlyDictionary<Key, Value>
		where Value : class
	{
		void Add(Key key, Value value);
	}
}
