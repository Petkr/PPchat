using System;

namespace PPchatLibrary
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
