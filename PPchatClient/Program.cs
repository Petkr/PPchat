namespace PPchatClient
{
	class Program
	{
		static void Main(string[] _)
		{
			using var client = new Client();
			client.AcceptCommands();
		}
	}
}
