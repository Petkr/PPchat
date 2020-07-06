using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandsInfo : ISimpleReadonlyDictionary<string, ICommandArgumentCountReadonlyDictionary>
	{
		ICommandDescriptor NotFoundCommand { get; }
		ICommandDescriptor BadArgumentCountCommand { get; }
	}
}
