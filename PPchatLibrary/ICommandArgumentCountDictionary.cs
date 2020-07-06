using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandArgumentCountDictionary : ICommandArgumentCountReadonlyDictionary, ISimpleDictionary<int, ICollection<ICommandDescriptor>>
	{ }
}
