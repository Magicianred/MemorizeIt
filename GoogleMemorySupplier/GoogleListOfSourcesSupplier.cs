using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Spreadsheets;
using MemorizeIt;

namespace GoogleMemorySupplier
{
    public class GoogleListOfSourcesSupplier : IListOfSourcesSupplier
    {
        private readonly string applicationName = "MemorizeIt";
        private readonly string userName;
        private readonly string password;

        public GoogleListOfSourcesSupplier(string applicationName, string userName, string password)
        {
            this.applicationName = applicationName;
            this.userName = userName;
            this.password = password;
        }

        public GoogleListOfSourcesSupplier(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public IEnumerable<string> GetSourcesList()
        {
            var retval = new List<string>();
            SpreadsheetsService service = new SpreadsheetsService(applicationName);
            service.setUserCredentials(userName, password);

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = applicationName;

            SpreadsheetFeed feed = service.Query(query);

            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                retval.Add(entry.Title.Text);
            }
            return retval;
        }
    }
}
