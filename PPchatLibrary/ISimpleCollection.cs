using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ISimpleCollection<T> : IEnumerable<T>
	{
		void Add(T item);
	}
}
