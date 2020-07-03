using System.Net;
using PPchatLibrary;

namespace PPchatClient
{
	class ClientConnection : Connection<ClientConnection>,
		IPacketHandler<MessageForClientPacket>
	{
		public Client Client => (Client)Application;

		public ClientConnection(Client client, IPAddress address, int port)
			: base(client, address, port)
		{ }

		public void Handle(MessageForClientPacket packet)
		{
			Application.Write(packet.Message);
		}
	}
}
