using System.Net;
using System.Net.Sockets;
using PPnetwork;
using PPchatPackets;

namespace PPchatClient
{
	/// <summary>
	/// Connection for the Client.
	/// </summary>
	class ClientConnection : Connection<Client, ClientConnection>,
		IPacketHandler<MessageForClientPacket>
	{
		/// <summary>
		/// Creates a TcpClient and connects it to <paramref name="ipAddress"/> on <see cref="port"/>.
		/// </summary>
		/// <returns>A connected TcpClient</returns>
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

		public override void HandleAbruptConnectionClose()
		{
			Application.Write("server abruptly terminated the connection");
		}

		public override void HandleNormalConnectionClose(string reason)
		{
			Application.Write($"server terminated the connection, reason: {reason}");
		}
	}
}
