using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandArgumentCountReadonlyDictionary : ISimpleReadonlyDictionary<int, IEnumerable<ICommandDescriptor>>
	{
		ICommandDescriptor? GetOneIfOneLongArgument { get; }
	}
}
