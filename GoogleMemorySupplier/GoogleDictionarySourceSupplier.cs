using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.GData.Client.ResumableUpload;
using Google.GData.Documents;
using Google.GData.Spreadsheets;
using MemorizeIt.DictionarySourceSupplier;
using MemorizeIt.Model;
using SpreadsheetQuery = Google.GData.Spreadsheets.SpreadsheetQuery;

/*
using Google.GData.Client;
using Google.GData.Spreadsheets;*/

namespace GoogleDictionarySupplier
{
    public class GoogleDictionarySourceSupplier : IDictionarySourceSupplier
    {
        private readonly string applicationName = "MemorizeIt";
        private readonly string spreadsheetName = "MemorizeIt";
        private readonly string userName;
        private readonly string password;

        public GoogleDictionarySourceSupplier(string userName, string password, string spreadsheetName = null)
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

        public DictionaryTable Download(string sheetName)
        {
            var service = new SpreadsheetsService(applicationName);
            WorksheetEntry worksheet = GetWorksheetEntres(service).FirstOrDefault(e => e.Title.Text == sheetName) as WorksheetEntry;
            if (worksheet == null)
                return null;
            List<DictionaryItem> retval = new List<DictionaryItem>();

            // Fetch the cell feed of the worksheet.
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            cellQuery.MaximumColumn = 2;

            CellFeed cellFeed = service.Query(cellQuery);

            for (int i = 0; i < cellFeed.Entries.Count; i = i + 2)
            {
                retval.Add(
                    new DictionaryItem(new string[]
                        {((CellEntry) cellFeed.Entries[i]).Value, ((CellEntry) cellFeed.Entries[i + 1]).Value}));
            }
            return new DictionaryTable(sheetName, retval.ToArray());
        }

        public void CreateTemplate()
        {
            DocumentsService service = new DocumentsService(applicationName);

            DocumentEntry entry = new DocumentEntry();

            // Set the document title
            entry.Title.Text = spreadsheetName;
            entry.IsSpreadsheet = true;

            // Set the media source
            entry.MediaSource = new MediaFileSource(GetStreamWithTemplate(), applicationName, "text/csv");

            // Define the resumable upload link
            Uri createUploadUrl = new Uri("https://docs.google.com/feeds/upload/create-session/default/private/full");
            AtomLink link = new AtomLink(createUploadUrl.AbsoluteUri);
            link.Rel = ResumableUploader.CreateMediaRelation;
            entry.Links.Add(link);

            // Set the service to be used to parse the returned entry
            entry.Service = service;

            // Instantiate the ResumableUploader component.
            ResumableUploader uploader = new ResumableUploader();


            // Start the upload process
            uploader.Insert(new ClientLoginAuthenticator(applicationName,ServiceNames.Documents,userName,password), entry);
        }

        private Stream GetStreamWithTemplate()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(
                "word1,translation of word1" + System.Environment.NewLine +
                "word2,translation of word2" + System.Environment.NewLine +
                "word3,translation of word3");
            writer.Flush();
            stream.Position = 0;
            return stream;

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
            var woorkSheets =
                feed.Entries.OfType<SpreadsheetEntry>()
                    .Where(spreadsheet => spreadsheet.Authors.Any(author => author.Email ==userName || author.Name== userName))
                    .ToList();
            return
                woorkSheets
                    .SelectMany(entry => entry.Worksheets.Entries.OfType<WorksheetEntry>());
        }
    }
}
