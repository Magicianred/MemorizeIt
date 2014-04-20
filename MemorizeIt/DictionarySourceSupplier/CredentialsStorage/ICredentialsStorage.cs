namespace MemorizeIt.DictionarySourceSupplier.CredentialsStorage
{
	public interface ICredentialsStorage
	{
		bool IsLoggedIn{ get;}
		void LogIn(string login, string password);
		void LogOut();
		Credentials GetCurrentUser();

	}
}

