using System;

namespace MemorizeIt
{
	public class Credentials
	{
		public Credentials (string login, string password)
		{
			this.Login = login;
			this.Password = password;
		}

		public string Login{ get; private set;}
		public string Password{ get; private set;}
	}
}

