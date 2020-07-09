using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	class PacketsSniffer<Connection> : BasicSniffer<Type, IInvoker<IConnection, IPacket>, Connection>
		where Connection : IConnection
	{
		public PacketsSniffer()
			: base(new Dictionary<Type, IInvoker<IConnection, IPacket>>(), typeof(IPacketHandler<>))
		{ }

		protected override void Handle(Type packetType)
			=> Add(packetType, new BasicDescriptor<IConnection, IPacket, Connection>(typeof(IPacketHandler<>), packetType));
	}
}
