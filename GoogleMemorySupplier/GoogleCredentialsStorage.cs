using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Spreadsheets;
using MemorizeIt;
using MemorizeIt.CredentialsStorage;

namespace GoogleMemorySupplier
{
    public class GoogleCredentials : ICredentialsStorage
    {
        private readonly ICredentialsStorage internalCredentials;
        private readonly string applicationName = "MemorizeIt";

        public GoogleCredentials()
        {
            internalCredentials=new InMemoryCredentialStorage();
        }

        public GoogleCredentials(string applicationName):this()
        {
            this.applicationName = applicationName;
        }

        public bool IsLoggedIn {
            get { return internalCredentials.IsLoggedIn; }
        }
        public void LogIn(string login, string password)
        {
            SpreadsheetsService service = new SpreadsheetsService(applicationName);
            service.setUserCredentials(login, password);

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = applicationName;
            try
            {

                SpreadsheetFeed feed = service.Query(query);
                if(feed.Entries.Count==0)
                    throw new CredentialsException("spreadsheet is absent");
                internalCredentials.LogIn(login, password);
            }
            catch (Exception e)
            {
                throw new CredentialsException(e.Message);
            }

        }

        public void LogOut()
        {
            internalCredentials.LogOut();
        }

        public Credentials GetCurrentUser()
        {
            return internalCredentials.GetCurrentUser();
        }
    }
}
