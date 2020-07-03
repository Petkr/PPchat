namespace PPchatClient
{
	class Program
	{
		static void Main(string[] _)
		{
			var client = new Client();
			client.AcceptCommands();
		}
	}
}
