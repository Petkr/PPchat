using System.Diagnostics.CodeAnalysis;

namespace PPchatLibrary
{
	interface IInfo<From, out To>
		where To : class
	{
		To? GetInfo(From from);
	}
}
