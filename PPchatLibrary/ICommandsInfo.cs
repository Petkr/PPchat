using System.Collections.Generic;

namespace PPchatLibrary
{
	interface ICommandsInfo : IInfo<string, IEnumerable<ICommandDescriptor>>
	{
		ICommandDescriptor NotFoundCommand { get; }
		ICommandDescriptor BadArgumentCountCommand { get; }
	}
}
