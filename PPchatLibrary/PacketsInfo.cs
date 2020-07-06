using System;

namespace PPchatLibrary
{
	class PacketsInfo<Connection> : BasicInfo<Type, IInvoker<IConnection, IPacket>, Connection>
		where Connection : IConnection
	{
		public PacketsInfo()
			: base(typeof(IPacketHandler<>))
		{ }

		protected override void Handle(Type packetType)
			=> Add(packetType, new BasicDescriptor<IConnection, IPacket, Connection>(typeof(IPacketHandler<>), packetType));
	}
}
