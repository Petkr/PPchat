namespace PPchatLibrary
{
	interface ISimpleReadonlyDictionary<Key, out Value>
		where Value : class
	{
		Value? GetValue(Key from);
	}
}
