using System;

namespace PPchatLibrary
{
	class PacketParser<Connection> : IParser<IConnection, IPacket, IPacket>
		where Connection : IConnection
	{
		static readonly IInfo<Type, IInvoker<IConnection, IPacket>> packetInfo = new PacketsInfo<Connection>();

		public (IInvoker<IConnection, IPacket>, IPacket) Parse(IPacket input)
			=> (packetInfo.GetInfo(input.GetType())!, input);
	}
}
