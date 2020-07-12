using System;
using PPnetwork;

namespace PPchatPackets
{
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
