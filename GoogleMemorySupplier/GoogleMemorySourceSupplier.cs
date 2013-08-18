using System.Collections;
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
    public class GoogleMemorySourceSupplier : IMemorySourceSupplier
    {
        private readonly string applicationName = "MemorizeIt";
        private readonly string spreadsheetName = "MemorizeIt";
        private readonly string userName;
        private readonly string password;

        public GoogleMemorySourceSupplier(string userName, string password, string spreadsheetName = null)
        {
            if (!string.IsNullOrEmpty(spreadsheetName))
                this.spreadsheetName = spreadsheetName;
            this.userName = userName;
            this.password = password;
        }

        public IEnumerable<string> GetSourcesList()
        {
            var retval = new List<string>();


            // Iterate through each worksheet in the spreadsheet.
            foreach (WorksheetEntry entry in GetWorksheetEntres())
            {
                retval.Add(entry.Title.Text);
            }

            return retval;
        }

        public MemoryTable Download(string sheetName)
        {
            var service = new SpreadsheetsService(applicationName);
            WorksheetEntry worksheet = GetWorksheetEntres(service).FirstOrDefault(e => e.Title.Text == sheetName) as WorksheetEntry;
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
            return new MemoryTable(sheetName, retval.ToArray());
        }


        private IEnumerable<WorksheetEntry> GetWorksheetEntres(SpreadsheetsService service = null)
        {
            if (service == null)
                service = new SpreadsheetsService(applicationName);
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

            return
                feed.Entries.OfType<SpreadsheetEntry>()
                    .SelectMany(entry => entry.Worksheets.Entries.OfType<WorksheetEntry>());
        }
    }
}
