using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PPchatLibrary
{
	public class MemoryStringComparer : IComparer<ReadOnlyMemory<char>>
	{
		public int Compare([AllowNull] ReadOnlyMemory<char> x, [AllowNull] ReadOnlyMemory<char> y)
			=> x.Span.SequenceCompareTo(y.Span);
	}
}
