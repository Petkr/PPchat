using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using PPnetwork;
using PPchatPackets;

namespace PPchatClient
{
	class Client : Application<Client>, IDisposable,
		ICommandHandler<ChangePortCommandArgument>,
		ICommandHandler<ConnectCommandArgument>,
		ICommandHandler<DefaultConnectCommandArgument>,
		ICommandHandler<DisconnectCommandArgument>,
		ICommandHandler<PrintPortCommandArgument>,
		ICommandHandler<SayCommandArgument>,
		ICommandHandler<SimpleConnectAddressCommandArgument>,
		ICommandHandler<SimpleConnectPortCommandArgument>,
		ICommandHandler<SimpleConnectSavedServerCommandArgument>,
		ICommandHandler<SaveServerCommandArgument>,
		ICommandHandler<ListSavedServersCommandArgument>
	{
		/// <summary>
		/// The one connection to the server. Null if not connected.
		/// </summary>
		IConnection? connection;
		bool Connected => connection != null;

		/// <summary>
		/// If the Client is connected returns an <see cref="IEnumerable{T}"/> with the Connection, otherwise an empty one.
		/// </summary>
		protected override IEnumerable<IConnection> Connections => connection?.AsSingleEnumerable() ?? Enumerable.Empty<IConnection>();

		protected override string ExitMessage => "disconnecting because the client is shutting down";

		// Settings
		const string settingsPath = "settings";
		int defaultPort;
		readonly IDictionary<ReadOnlyMemory<char>, (IPAddress, int)> savedServers;

		/// <summary>
		/// Serializes <see cref="savedServers"/> to <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream"></param>
		void SerializeSavedServers(SimpleSerializerStream stream)
		{
			stream.Write(savedServers.Count);
			foreach (var (key, value) in savedServers)
			{
				stream.Write(key);
				stream.Write(value);
			}
		}

		/// <summary>
		/// Deserializes the saved servers into an <see cref="IEnumerable{T}"/> of (name, (address, port))
		/// </summary>
		/// <returns>key-value pairs from the stream</returns>
		static IEnumerable<(ReadOnlyMemory<char> key, (IPAddress, int) value)> DeserializeSavedServersImplementation(SimpleSerializerStream serializer)
		{
			int count;
			try
			{
				count = serializer.Read<int>();
			}
			catch (SimpleSerializerEndOfStreamException)
			{
				yield break;
			}

			(ReadOnlyMemory<char> key, (IPAddress, int) value) pair;

			for (int i = 0; i != count; ++i)
			{
				pair.key = serializer.Read<Memory<char>>();
				pair.value = serializer.ReadPair<IPAddress, int>();
				yield return pair;
			}
		}

		/// <summary>
		/// Deserializes the saved servers from <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		static IDictionary<ReadOnlyMemory<char>, (IPAddress, int)> DeserializeSavedServers(SimpleSerializerStream stream)
		{
			IDictionary<ReadOnlyMemory<char>, (IPAddress, int)> saved_servers =
				new SortedDictionary<ReadOnlyMemory<char>, (IPAddress, int)>(new MemoryStringComparer());

			foreach (var (key, value) in DeserializeSavedServersImplementation(stream))
				saved_servers.Add(key, value);

			return saved_servers;
		}

		public Client()
		{
			using var stream = new SimpleSerializerStream(new FileStream(settingsPath, FileMode.OpenOrCreate));
			try
			{
				defaultPort = stream.Read<int>();
			}
			catch (SimpleSerializerEndOfStreamException)
			{
				defaultPort = 2048;
			}
			savedServers = DeserializeSavedServers(stream);
		}

		/// <summary>
		/// Dumps the settings on disk.
		/// </summary>
		public void Dispose()
		{
			File.Delete(settingsPath);
			using var stream = new SimpleSerializerStream(new FileStream(settingsPath, FileMode.CreateNew));
			stream.Write(defaultPort);
			SerializeSavedServers(stream);
		}

		public override void RemoveConnection(IConnection _)
		{
			// the Client has only one Connection (at most)
			connection = null;
		}

		/// <summary>
		/// Changes the <see cref="defaultPort"/> to <paramref name="port"/>.
		/// </summary>
		internal void ChangePort(int port)
		{
			Write($"the default port was changed from: {defaultPort}, to: {port}");
			defaultPort = port;
		}

		protected override void ClearConnections()
		{
			// again, just one Connection
			connection = null;
		}

		public void Handle(ChangePortCommandArgument argument)
		{
			Write($"the default port was changed from: {defaultPort}, to: {argument.port}");
			defaultPort = argument.port;
		}

		public void Handle(PrintPortCommandArgument _)
		{
			Write($"the default port is: {defaultPort}");
		}

		public void Handle(DisconnectCommandArgument _)
		{
			if (Connected)
			{
				CloseAllConnections("disconnecting by command");
				Write("disconnected");
			}
			else
				Write("you're not connected to any server");
		}

		public void Connect(IPAddress ipAddress, int port)
		{
			try
			{
				connection = new ClientConnection(this, ipAddress, port);
				Write("enter your username:");
				var username = Read();
				connection.Stream.Write(new LoginPacket(username));
			}
			catch
			{
				connection = null;
				Write("couldn't connect");
			}
		}
		public void Handle(ConnectCommandArgument argument)
		{
			Connect(argument.ipAddress, argument.port);
		}
		public void Handle(SimpleConnectPortCommandArgument argument)
		{
			Connect(IPAddress.Loopback, argument.Port);
		}
		public void Handle(SimpleConnectAddressCommandArgument argument)
		{
			Connect(argument.Address, defaultPort);
		}
		public void Handle(DefaultConnectCommandArgument _)
		{
			Connect(IPAddress.Loopback, defaultPort);
		}

		void SendMessage(ReadOnlyMemory<char> message)
		{
			if (Connected)
			{
				connection!.Stream.Write(new MessageForServerPacket(new string(message.Span)));
			}
			else
			{
				Write("you are not connected");
			}
		}

		public void Handle(SayCommandArgument argument) => SendMessage(argument.Message);

		public override void Handle(NotFoundCommandArgument argument) => SendMessage(argument.Input);

		public void Handle(SaveServerCommandArgument argument)
		{
			if (savedServers.TryAdd(argument.ServerName, (argument.Address, argument.Port)))
				Write($"successfully saved new server {argument.ServerName}");
			else
				Write($"there is already a saved server named {argument.ServerName}");
		}

		public void Handle(SimpleConnectSavedServerCommandArgument argument)
		{
			if (savedServers.TryGetValue(argument.ServerName, out (IPAddress address, int port) pair))
				Connect(pair.address, pair.port);
			else
				Write($"There is no saved server named {argument.ServerName}");
		}

		public void Handle(ListSavedServersCommandArgument argument)
		{
			if (savedServers.Any())
			{
				Write("your saved servers are:");
				foreach (var (name, (address, port)) in savedServers)
					Write($"{name}: {address}, {port}");
			}
			else
			{
				Write("you don't have any saved servers");
			}
		}
	}
}
