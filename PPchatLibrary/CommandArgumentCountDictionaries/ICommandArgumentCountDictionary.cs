using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandArgumentCountDictionary : ICommandArgumentCountReadonlyDictionary, ISimpleDictionary<int, ISimpleCollection<ICommandDescriptor>>
	{
		new ISimpleCollection<ICommandDescriptor>? GetValue(int from);

		IEnumerable<ICommandDescriptor>? ISimpleReadonlyDictionary<int, IEnumerable<ICommandDescriptor>>.GetValue(int from)
			=> GetValue(from);

		void AddIfOneLongArgument(ICommandDescriptor descriptor);
	}
}
