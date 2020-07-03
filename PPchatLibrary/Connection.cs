using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace PPchatLibrary
{
	public abstract class Connection<ApplicationConnection> : IConnection,
		IPacketHandler<EndPacket>

		where ApplicationConnection : IConnection
	{
		public IApplication Application { get; }
		readonly TcpClient tcpClient;
		public IReaderWriter<IPacket> Stream { get; }
		readonly Thread thread;
		bool should_close = false;

		static readonly IInvoker<IConnection, IPacket> Parser = new PacketParser<ApplicationConnection>();

		static TcpClient CreateClientAndConnect(IPAddress ipAddress, int port)
		{
			var client = new TcpClient();
			client.Connect(ipAddress, port);
			return client;
		}
		public Connection(IApplication application, TcpClient tcpClient)
		{
			if (!tcpClient.Connected)
				throw new Exception("The client provided to the constructor should be connected already. Use the other overload to connect to a particular address and port");

			Application = application;
			this.tcpClient = tcpClient;
			Stream = new FormatterStream<BinaryFormatter, IPacket>(tcpClient.GetStream());
			thread = new Thread(Handle);
			thread.Start();
		}
		public Connection(IApplication application, IPAddress ipAddress, int port)
			: this(application, CreateClientAndConnect(ipAddress, port))
		{}

		public void Close()
		{
			lock (this)
			{
				should_close = true;
			}
			tcpClient.Close();
			thread.Join();
		}

		public void Handle()
		{
			try
			{
				while (true)
					Parser.Invoke(this, Stream.Read());
			}
			catch (EndException e)
			{
				Application.HandleNormalConnectionClose(this, e.Reason);
				RemoveConnection();
			}
			catch
			{
				bool sc;
				lock (this)
				{
					sc = should_close;
				}
				if (!sc)
				{
					Application.HandleAbruptConnectionClose(this);
					RemoveConnection();
				}
			}
		}

		void RemoveConnection()
		{
			Application.RemoveConnection(this);
		}

		public void Handle(EndPacket packet)
		{
			throw new EndException(packet.Reason);
		}
	}
}
