using System;

namespace PPchatLibrary
{
	interface ICommandsSniffer : ISimpleReadonlyDictionary<ReadOnlyMemory<char>, ICommandArgumentCountReadonlyDictionary>
	{
		ICommandDescriptor NotFoundCommand { get; }
		ICommandDescriptor BadArgumentCountCommand { get; }
	}
}
