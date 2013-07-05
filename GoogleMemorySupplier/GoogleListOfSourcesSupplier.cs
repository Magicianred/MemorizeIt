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
        private readonly string spreadsheetName = "MemorizeIt";
        private readonly string userName;
        private readonly string password;

        public GoogleListOfSourcesSupplier(string spreadsheetName, string userName, string password)
        {
            this.spreadsheetName = spreadsheetName;
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
            query.Title = spreadsheetName;

            SpreadsheetFeed feed = service.Query(query);
            var spreadsheet = (SpreadsheetEntry)feed.Entries[0];

            // Make a request to the API to fetch information about all
            // worksheets in the spreadsheet.
            WorksheetFeed wsFeed = spreadsheet.Worksheets;

            // Iterate through each worksheet in the spreadsheet.
            foreach (WorksheetEntry entry in wsFeed.Entries)
            {
                retval.Add(entry.Title.Text);
            }
            return retval;
        }
    }
}
