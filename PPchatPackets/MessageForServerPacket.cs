using System;
using PPnetwork;

namespace PPchatPackets
{
	/// <summary>
	/// Message Packet sent by the Client to other Clients (logically, not literally).
	/// </summary>
	[Serializable]
	public readonly struct MessageForServerPacket : IPacket
	{
		public readonly string Message;

		public MessageForServerPacket(string message)
		{
			Message = message;
		}
	}
}
