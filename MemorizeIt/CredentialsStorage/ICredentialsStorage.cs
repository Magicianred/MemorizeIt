using System;

namespace MemorizeIt
{
	public interface ICredentialsStorage
	{
		bool IsLoggedIn{ get;}
		void LogIn(string login, string password);
		void LogOut();
		Credentials GetCurrentUser();

	}
}

