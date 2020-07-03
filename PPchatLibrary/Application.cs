using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	public abstract class Application<SpecificApplication> : IApplication,
		ICommandHandler<NotFoundCommandArgument>,
		ICommandHandler<BadArgumentCountCommandArgument>,
		ICommandHandler<ExitCommandArgument>

		where SpecificApplication : IApplication
	{
		public void Write(string s)
		{
			Console.WriteLine(s);
		}
		public string Read()
		{
			return Console.ReadLine()!;
		}
		protected abstract IEnumerable<IConnection> Connections { get; }
		protected abstract void ClearConnections();
		public abstract void RemoveConnection(IConnection connection);

		static readonly IInvoker<IApplication, string> Parser = new CommandParser<SpecificApplication>();

		protected abstract void HandleAfterExit();
		public abstract void HandleAbruptConnectionClose(IConnection connection);
		public abstract void HandleNormalConnectionClose(IConnection connection, string reason);

		protected abstract string ExitMessage { get; }

		protected void CloseAllConnections(string reason)
		{
			foreach (var connection in Connections)
				connection.Stream.Write(new EndPacket(reason));			

			foreach (var connection in Connections)
				connection.Close();

			ClearConnections();
		}

		public void AcceptCommands()
		{
			string s;

			while (true)
			{
				try
				{
					s = Read().Trim();
					if (s != "")
						Parser.Invoke(this, s);
				}
				catch (ExitCommandException)
				{
					CloseAllConnections(ExitMessage);
					HandleAfterExit();
					break;
				}
				catch (ParseException pe)
				{
					Write(pe.Message);
				}
			}
		}

		public void Handle(BadArgumentCountCommandArgument argument)
		{
			Write($"bad argument count: {argument.count}");
		}

		public void Handle(ExitCommandArgument _)
		{
			throw new ExitCommandException();
		}

		public abstract void Handle(NotFoundCommandArgument argument);
	}
}
