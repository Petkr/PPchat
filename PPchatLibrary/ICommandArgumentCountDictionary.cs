using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandArgumentCountDictionary : ICommandArgumentCountReadonlyDictionary, ISimpleDictionary<int, ISimpleCollection<ICommandDescriptor>>
	{
		IEnumerable<ICommandDescriptor>? ISimpleReadonlyDictionary<int, IEnumerable<ICommandDescriptor>>.GetValue(int from)
			=> ((ISimpleDictionary<int, ICollection<ICommandDescriptor>>)this).GetValue(from);

		void AddIfOneLongArgument(ICommandDescriptor descriptor);
	}
}
