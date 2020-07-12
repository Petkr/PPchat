using PPnetwork;

namespace PPchatClient
{
	class Program
	{
		static void Main(string[] _)
		{
			NativeParsing.SetEncoding();

			using var client = new Client();
			client.AcceptCommands();
		}
	}
}
