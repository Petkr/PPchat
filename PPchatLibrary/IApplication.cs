namespace PPchatLibrary
{
	public interface IApplication : IReaderWriter<string>
	{
		void HandleNormalConnectionClose(IConnection connection, string reason);
		void HandleAbruptConnectionClose(IConnection connection);
		void RemoveConnection(IConnection connection);
	}
}
