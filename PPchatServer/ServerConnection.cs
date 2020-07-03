using System.Net.Sockets;
using PPchatLibrary;

namespace PPchatServer
{
	class ServerConnection : Connection<ServerConnection>,
		IPacketHandler<MessageForServerPacket>
	{
		public Server Server => (Server)Application;

		public ServerConnection(Server server, TcpClient tcpClient)
			: base(server, tcpClient)
		{ }

		public void Handle(MessageForServerPacket packet)
		{
			Server.HandleIncomingMessage(this, packet.Message);
		}
	}
}
