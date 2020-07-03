using System.Net.Sockets;
using PPchatLibrary;

namespace PPchatServer
{
	class ServerConnection : Connection<ServerConnection>,
		IPacketHandler<MessageForServerPacket>,
		IPacketHandler<LoginPacket>
	{
		string? username;

		public Server Server => (Server)Application;

		public ServerConnection(Server server, TcpClient tcpClient)
			: base(server, tcpClient)
		{ }

		public void Handle(MessageForServerPacket packet)
		{
			var message = $"{username} said: {packet.Message}";

			Application.Write(message);
			foreach (var connection in Server.OtherConnectionsThan(this))
				connection.Stream.Write(new MessageForClientPacket(message));
		}

		public void Handle(LoginPacket packet)
		{
			username = packet.Username;
			Stream.Write(new MessageForClientPacket($"hi, {username}!"));
			Application.Write($"user {username} logged in");
		}
	}
}
