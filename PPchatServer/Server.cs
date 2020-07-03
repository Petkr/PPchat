using PPchatLibrary;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace PPchatServer
{
	public class Server : Application<Server>,
		ICommandHandler<StartCommandArgument>,
		ICommandHandler<StopCommandArgument>
	{
		readonly TcpListener tcpListener;

		readonly ISet<IConnection> connections;
		protected override IEnumerable<IConnection> Connections => connections;

		Thread? acceptConnectionsThread;

		bool Running => acceptConnectionsThread != null;

		protected override string ExitMessage => "server shut down";

		public Server()
		{
			tcpListener = new TcpListener(IPAddress.Any, 2048);
			connections = new HashSet<IConnection>();
		}

		void AcceptConnections()
		{
			TcpClient tcpClient;
			while (true)
			{
				try
				{
					tcpClient = tcpListener.AcceptTcpClient();
				}
				catch (SocketException e)
				{
					if (e.SocketErrorCode != SocketError.Interrupted)
						Write("new client accepting stopped unexpectedly");
					break;
				}
				var connection = new ServerConnection(this, tcpClient);
				connection.Stream.Write(new MessageForClientPacket("server says hi!"));
				Write("a client connected");
				connections.Add(connection);
			}
		}

		void StopListening()
		{
			tcpListener.Stop();
			acceptConnectionsThread?.Join();
			acceptConnectionsThread = null;
		}

		public void HandleIncomingMessage(IConnection fromConnection, string message)
		{
			Write($"client said: {message}");

			foreach (var connection in Connections)
				if (connection != fromConnection)
					connection.Stream.Write(new MessageForClientPacket(message));
		}

		protected override void HandleAfterExit() => StopListening();

		public override void RemoveConnection(IConnection connection)
		{
			connections.Remove(connection);
		}

		protected override void ClearConnections()
		{
			connections.Clear();
		}

		public override void HandleAbruptConnectionClose(IConnection connection)
		{
			Write("connection with a client was abruptly terminated");
		}

		public override void HandleNormalConnectionClose(IConnection connection, string reason)
		{
			Write($"client disconnected, reason: {reason}");
		}

		public void Handle(StartCommandArgument _)
		{
			if (!Running)
			{
				tcpListener.Start();
				acceptConnectionsThread = new Thread(AcceptConnections);
				acceptConnectionsThread.Start();
				Write("server is now runnning");
			}
			else
				Write("already running");
		}

		public void Handle(StopCommandArgument _)
		{
			if (Running)
			{
				CloseAllConnections("server stopped by command");
				StopListening();
				Write("server has stopped");
			}
			else
				Write("not running");
		}

		static (string, bool did_cut) CutString(string s, int maxLength)
		{
			if (s.Length > maxLength)
				return (s.Substring(0, maxLength), true);
			else
				return (s, false);
		}

		static string CutStringFancy(string s, int maxLength, string to_append_if_cut)
		{
			var (s_out, did_cut) = CutString(s, maxLength);
			if (did_cut)
				return s_out + to_append_if_cut;
			else
				return s_out;
		}

		public override void Handle(NotFoundCommandArgument argument)
		{
			Write($"invalid command: {CutStringFancy(argument.Input, 20, "...")}");
		}
	}
}
