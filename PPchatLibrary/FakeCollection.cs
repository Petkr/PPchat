using System;
using System.Collections;
using System.Collections.Generic;

namespace PPchatLibrary
{
	class FakeCollection<T> : ISimpleCollection<T>
	{
		readonly IEnumerable<T> Enumerable;

		public FakeCollection(IEnumerable<T> enumerable)
			=> Enumerable = enumerable;
		public void Add(T _)
			=> throw new Exception();
		public IEnumerator<T> GetEnumerator()
			=> Enumerable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator()
			=> Enumerable.GetEnumerator();
	}
}
