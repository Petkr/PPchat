using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandArgumentCountReadonlyDictionary : ISimpleReadonlyDictionary<int, IEnumerable<ICommandDescriptor>>
	{
		ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument { get; }
	}
}
