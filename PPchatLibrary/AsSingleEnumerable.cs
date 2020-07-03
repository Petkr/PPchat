using System.Collections.Generic;

namespace PPchatLibrary
{
	public static class Extensions
    {
        public static IEnumerable<T> AsSingleEnumerable<T>(this T x)
        {
            yield return x;
        }
    }
}
