using System;

namespace MemorizeIt
{
	public class InMemoryCredentialStorage:ICredentialsStorage
	{
		private Credentials currentUser;
		#region ICredentialsStorage implementation

		public Void LogIn (String login, String password)
		{
			if (IsLoggedIn)
				throw new InvalidOperationException ();
			currentUser = new Credentials (login, password);
		}

		public Void LogOut ()
		{			
			if (!IsLoggedIn)
				throw new InvalidOperationException ();
			currentUser = null;
		}

		public Credentials GetCurrentUser ()
		{
			return currentUser;
		}

		public Boolean IsLoggedIn {
			get {
				return currentUser!=null;
			}
		}

		#endregion
	}
}

