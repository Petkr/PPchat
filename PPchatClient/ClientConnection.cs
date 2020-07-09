using System.Net;
using PPchatLibrary;

namespace PPchatClient
{
	class ClientConnection : Connection<Client, ClientConnection>,
		IPacketHandler<MessageForClientPacket>
	{
		public ClientConnection(Client client, IPAddress address, int port)
			: base(client, address, port)
		{ }

		public void Handle(MessageForClientPacket packet)
		{
			Application.Write(packet.Message);
		}
	}
}
