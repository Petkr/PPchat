namespace PPchatLibrary
{
	interface ICommandsSniffer : ISimpleReadonlyDictionary<string, ICommandArgumentCountReadonlyDictionary>
	{
		ICommandDescriptor NotFoundCommand { get; }
		ICommandDescriptor BadArgumentCountCommand { get; }
	}
}
