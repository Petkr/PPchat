using PPnetwork;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;

namespace PPchatServer
{
	class Server : Application<Server>,
		ICommandHandler<StartCommandArgument>,
		ICommandHandler<StartPortCommandArgument>,
		ICommandHandler<StopCommandArgument>
	{
		TcpListener? tcpListener;
		bool Running => tcpListener != null;

		/// <summary>
		/// Thread on which new Connections are accepted
		/// </summary>
		Thread? acceptConnectionsThread;


		readonly ISet<IConnection> connections;
		protected override IEnumerable<IConnection> Connections => connections;

		protected override string ExitMessage => "server shut down";

		public Server()
		{
			connections = new HashSet<IConnection>();
		}

		/// <summary>
		/// Loop that listens to new connecting Connections.
		/// </summary>
		void AcceptConnections()
		{
			TcpClient tcpClient;
			while (true)
			{
				try
				{
					tcpClient = tcpListener!.AcceptTcpClient();
				}
				catch (SocketException e)
				{
					if (e.SocketErrorCode != SocketError.Interrupted)
						Write("new client accepting stopped unexpectedly");
					break;
				}
				var connection = new ServerConnection(this, tcpClient);
				Write("a client connected");
				connections.Add(connection);
			}
		}

		/// <summary>
		/// Start accepting new Connections on <paramref name="port"/>.
		/// </summary>
		void StartListening(int port)
		{
			if (!Running)
			{
				tcpListener = new TcpListener(IPAddress.Any, port);
				tcpListener.Start();
				acceptConnectionsThread = new Thread(AcceptConnections);
				acceptConnectionsThread.Start();
				Write("server is now runnning");
			}
			else
				Write("already running");
		}

		/// <summary>
		/// Stops the accepting of new Connections.
		/// </summary>
		void StopListening()
		{
			tcpListener?.Stop();
			tcpListener = null;
			acceptConnectionsThread?.Join();
			acceptConnectionsThread = null;
		}

		/// <summary>
		/// Gets an <see cref="IEnumerable{T}"/> of all connected Connections to the Server other than <paramref name="connection"/>.
		/// </summary>
		public IEnumerable<IConnection> OtherConnectionsThan(IConnection connection) =>
			Connections.Where(x => x != connection);

		/// <summary>
		/// After the exit Command is entered, stop accepting new Connections.
		/// This is just a cleanup.
		/// </summary>
		protected override void HandleAfterExit() => StopListening();

		public override void RemoveConnection(IConnection connection)
		{
			connections.Remove(connection);
		}

		protected override void ClearConnections()
		{
			connections.Clear();
		}

		public void Handle(StartPortCommandArgument argument)
			=> StartListening(argument.Port);

		public void Handle(StartCommandArgument _)
			=> StartListening(2048);

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

		static (ReadOnlyMemory<char>, bool did_cut) CutString(ReadOnlyMemory<char> s, int maxLength)
		{
			if (s.Length > maxLength)
				return (s.Slice(0, maxLength), true);
			else
				return (s, false);
		}

		static ReadOnlyMemory<char> CutStringFancy(ReadOnlyMemory<char> s, int maxLength, ReadOnlyMemory<char> to_append_if_cut)
		{
			var (s_out, did_cut) = CutString(s, maxLength);
			if (did_cut)
				return string.Concat(s_out.Span, to_append_if_cut.Span).AsMemory();
			else
				return s_out;
		}
		
		public override void Handle(NotFoundCommandArgument argument)
		{
			Write($"invalid command: {CutStringFancy(argument.Input, 20, "...".AsMemory())}");
		}
	}
}
