using System;
using PPnetwork;

namespace PPchatPackets
{
	/// <summary>
	/// Message Packet sent by the Server to the Client.
	/// </summary>
	[Serializable]
	public readonly struct MessageForClientPacket : IPacket
	{
		public readonly string Message;

		public MessageForClientPacket(string message)
		{
			Message = message;
		}
	}
}
