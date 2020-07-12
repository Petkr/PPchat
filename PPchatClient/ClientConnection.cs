using System.Net;
using System.Net.Sockets;
using PPchatLibrary;
using PPchatPackets;

namespace PPchatClient
{
	class ClientConnection : Connection<Client, ClientConnection>,
		IPacketHandler<MessageForClientPacket>
	{
		static TcpClient CreateClientAndConnect(IPAddress ipAddress, int port)
		{
			var client = new TcpClient();
			client.Connect(ipAddress, port);
			return client;
		}

		public ClientConnection(Client client, IPAddress address, int port)
			: base(client, CreateClientAndConnect(address, port))
		{ }

		public void Handle(MessageForClientPacket packet)
		{
			Application.Write(packet.Message);
		}
	}
}
