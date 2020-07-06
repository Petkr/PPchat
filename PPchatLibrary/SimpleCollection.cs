using System.Collections;
using System.Collections.Generic;

namespace PPchatLibrary
{
	class SimpleCollection<Collection, T> : ISimpleCollection<T>
		where Collection : ICollection<T>, new()
	{
		readonly ICollection<T> collection;

		public SimpleCollection() => collection = new Collection();
		public void Add(T item) => collection.Add(item);
		public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
	}
}
