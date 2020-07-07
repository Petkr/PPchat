using PPchatLibrary;

namespace PPchatClient
{
	[Command("connect", 2)]
	public readonly struct SimpleConnectSavedServerCommandArgument : ICommandArgument
	{
		public readonly string ServerName;

		public SimpleConnectSavedServerCommandArgument(string serverName)
		{
			ServerName = serverName;
		}
	}
}
