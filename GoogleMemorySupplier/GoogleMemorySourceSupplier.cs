using System.Collections.Generic;
using System.Linq;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using MemorizeIt.MemorySourceSupplier;
using MemorizeIt.Model;
/*
using Google.GData.Client;
using Google.GData.Spreadsheets;*/

namespace GoogleMemorySupplier
{
    public class GoogleMemorySourceSupplier:IMemorySourceSupplier
    {
        private readonly string applicationName = "MemorizeIt";
        private readonly string spreadsheetName = "MemorizeIt";
        private readonly string sheetName;
        private readonly string userName;
        private readonly string password;

        public GoogleMemorySourceSupplier(string sheetName, string userName, string password, string spreadsheetName=null)
        {
            if (!string.IsNullOrEmpty(spreadsheetName))
                this.spreadsheetName = spreadsheetName;
            this.sheetName = sheetName;
            this.userName = userName;
            this.password = password;
        }

        public MemoryTable Download()
        {
            SpreadsheetsService service = new SpreadsheetsService(applicationName);
            service.setUserCredentials(userName, password);

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = spreadsheetName;
            // Make a request to the API and get all spreadsheets.
            SpreadsheetFeed feed = service.Query(query);

            if (feed.Entries.Count == 0)
            {
                return null;
            }

            // TODO: Choose a spreadsheet more intelligently based on your
            // app's needs.
            SpreadsheetEntry spreadsheet = (SpreadsheetEntry) feed.Entries[0];


            // Get the first worksheet of the first spreadsheet.
            // TODO: Choose a worksheet more intelligently based on your
            // app's needs.
            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            WorksheetEntry worksheet = wsFeed.Entries.FirstOrDefault(e => e.Title.Text == sheetName) as WorksheetEntry;
            if (worksheet == null)
                return null;


            List<MemoryItem> retval = new List<MemoryItem>();

            // Fetch the cell feed of the worksheet.
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            cellQuery.MaximumColumn = 2;

            CellFeed cellFeed = service.Query(cellQuery);

            for (int i = 0; i < cellFeed.Entries.Count; i = i + 2)
            {
                retval.Add(
                    new MemoryItem(new string[]
                        {((CellEntry) cellFeed.Entries[i]).Value, ((CellEntry) cellFeed.Entries[i + 1]).Value}));
            }
            return new MemoryTable(applicationName, retval.ToArray());
        }
    }
}
