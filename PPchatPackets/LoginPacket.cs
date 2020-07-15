using System;
using PPnetwork;

namespace PPchatPackets
{
	/// <summary>
	/// Packet used to send the username after connecting.
	/// </summary>
	[Serializable]
	public readonly struct LoginPacket : IPacket
	{
		public readonly string Username;

		public LoginPacket(string username)
		{
			Username = username;
		}
	}
}
