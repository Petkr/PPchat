using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	class BasicCommandArgumentCountDictionary :
		SimpleDictionary<Dictionary<int, ISimpleCollection<ICommandDescriptor>>, int, ISimpleCollection<ICommandDescriptor>>,
		ICommandArgumentCountDictionary
	{
		public ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument
			=> null;

		public void AddIfOneLongArgument(ICommandDescriptor _)
			=> throw new Exception();
	}
}
