namespace PPchatServer
{
	class Program
	{
		static void Main(string[] _)
		{
			var server = new Server();
			server.AcceptCommands();
		}
	}
}
