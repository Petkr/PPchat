using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace PPchatLibrary
{
	public abstract class Connection<SpecificApplication, ApplicationConnection> : IConnection,
		IPacketHandler<EndPacket>

		where SpecificApplication : IApplication
		where ApplicationConnection : IConnection
	{
		public SpecificApplication Application { get; }
		readonly TcpClient tcpClient;
		public IReaderWriter<IPacket> Stream { get; }
		readonly Thread thread;
		bool should_close = false;

		static readonly IInvoker<IConnection, IPacket> Parser = new PacketParser<ApplicationConnection>();

		protected Connection(SpecificApplication application, TcpClient tcpClient)
		{
			if (!tcpClient.Connected)
				throw new Exception("The client provided to the constructor should be connected already.");

			Application = application;
			this.tcpClient = tcpClient;
			Stream = new FormatterStream<BinaryFormatter, IPacket>(tcpClient.GetStream());
			thread = new Thread(Handle);
			thread.Start();
		}

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
