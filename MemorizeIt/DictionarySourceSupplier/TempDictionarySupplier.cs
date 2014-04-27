using System;
using MemorizeIt.DictionarySourceSupplier;
using MemorizeIt.Model;
using System.Linq;
using MemorizeIt.Model;
using System.Collections.Generic;


namespace MemorizeIt
{
	public class TempDictionarySupplier:IDictionarySourceSupplier
	{
		#region IDictionarySourceSupplier implementation

		public MemorizeIt.Model.DictionaryTable Download (string sheetName)
		{
			string[][] datas = null;
			if (sheetName == "States-Capitals") {
				datas = StatesCapitals ();
			}
			if (sheetName == "Rus-Eng") {
				datas = RusEng ();
			}
			if(datas==null)
				return null	;

			List<DictionaryItem> retval = new List<DictionaryItem>();
			foreach (var row in datas) {
				retval.Add (CreateDictionaryItem (row));
			}

			return	new DictionaryTable (sheetName, retval.ToArray ());
		}
		private DictionaryItem CreateDictionaryItem(string[] items){
			return new DictionaryItem (Guid.NewGuid(), items, 0); 
		}

		public System.Collections.Generic.IEnumerable<string> GetSourcesList ()
		{
			return new string[]{ "States-Capitals","Rus-Eng" };
		}

		public void CreateTemplate ()
		{
			throw new InvalidOperationException ("template creation is not avalible for public sources");
		}

		#endregion

		public TempDictionarySupplier ()
		{
		}
		private string[][] StatesCapitals(){
			return new string[][] {
				new []{ "Alabama", "Montgomery" }, 
				new []{ "Alaska", "Juneau" }, 
				new []{ "Arizona", "Phoenix" }, 
				new []{ "Arkansas", "Little Rock" }, 
				new []{ "California", "Sacramento" }, 
				new []{ "Colorado", "Denver" }, 
				new []{ "Connecticut", "Hartford" }, 
				new []{ "Delaware", "Dover" }, 
				new []{ "Florida", "Tallahassee" },
				new []{ "Georgia", "Atlanta" }, 
				new []{ "Hawaii", "Honolulu" }, 
				new []{ "Idaho", "Boise" }, 
				new []{ "Illinois", "Springfield" }, 
				new []{ "Indiana", "Indianapolis" }, 
				new []{ "Iowa", "Des Moines" }, 
				new []{ "Kansas", "Topeka" }, 
				new []{ "Kentucky", "Frankfort" }, 
				new []{ "Louisiana", "Baton Rouge" }, 
				new []{ "Maine", "Augusta" }, 
				new []{ "Maryland", "Annapolis" }, 
				new []{ "Massachusetts", "Boston" }, 
				new []{ "Michigan", "Lansing" }, 
				new []{ "Minnesota", "Saint Paul" }, 
				new []{ "Mississippi", "Jackson" }, 
				new []{ "Missouri", "Jefferson City" }, 
				new []{ "Montana", "Helena" }, 
				new []{ "Nebraska", "Lincoln" }, 
				new []{ "Nevada", "Carson City" }, 
				new []{ "New Hampshire", "Concord" }, 
				new []{ "New Jersey", "Trenton" }, 
				new []{ "New Mexico", "Santa Fe" }, 
				new []{ "New York", "Albany" }, 
				new []{ "North Carolina", "Raleigh" }, 
				new []{ "North Dakota", "Bismarck" }, 
				new []{ "Ohio", "Columbus" }, 
				new []{ "Oklahoma", "Oklahoma City" },
				new []{ "Oregon", "Salem" }, 
				new []{ "Pennsylvania", "Harrisburg" }, 
				new []{ "Rhode Island", "Providence" }, 
				new []{ "South Carolina", "Columbia" }, 
				new []{ "South Dakota", "Pierre" }, 
				new []{ "Tennessee", "Nashville" }, 
				new []{ "Texas", "Austin" }, 
				new []{ "Utah", "Salt Lake City" }, 
				new []{ "Vermont", "Montpelier" }, 
				new []{ "Virginia", "Richmond" }, 
				new []{ "Washington", "Olympia" }, 
				new []{ "West Virginia", "Charleston" }, 
				new []{ "Wisconsin", "Madison" }, 
				new []{ "Wyoming", "Cheyenne" }
			};
		}
		private string[][] RusEng(){
			return new string[][] {
					new string[]{ "Yes", "Da" },
					new string[]{ "No", "Net" },
					new string[]{ "Thank you", "Spasibo" },
					new string[]{ "Please", "Pazhaluysta" },
					new string[]{ "Excuse me", "Izvinite" },
					new string[]{ "Hello", "Zdravstvuyte" },
					new string[]{ "Goodbye", "Do svidaniya" }, 
					new string[]{ "I do not understand", "Ya ne ponimayu" },
					new string[]{ "Do you speak ...", "Vy govorite po-..." }, 
					new string[]{ "I", "Ya" }, 
					new string[]{ "We", "Mui" }, 
				new string[]{ "You", "Ty" }, 
					new string[]{ "They", "Oni" }, 
					new string[]{ "What is your name?", "Kak vas zovut?" }, 
					new string[]{ "Nice to meet you.", "Ochen priyatno." }, 
					new string[]{ "How are you?", "Kak dela?" }, 
					new string[]{ "Good", "Horosho" }, 
					new string[]{ "Bad", "Ploho" }, 
					new string[]{ "Wife", "Zhena" }, 
					new string[]{ "Husband", "Muzh" }, 
					new string[]{ "Daughter", "Doch" }, 
					new string[]{ "Son", "Syn" }, 
					new string[]{ "Mother", "Mat" }, 
					new string[]{ "Father", "Otets" }, 
					new string[]{ "Friend", "Drug" }, 
					new string[]{ "zero", "nol" }, 
					new string[]{ "one", "odin" }, 
					new string[]{ "two", "dva" }, 
					new string[]{ "three", "tri" }, 
					new string[]{ "four", "chetyre" }, 
					new string[]{ "five", "pyat" }, 
					new string[]{ "six", "shest" }, 
					new string[]{ "seven", "sem" }, 
					new string[]{ "eight", "vosem" }, 
					new string[]{ "nine", "devyat" }, 
					new string[]{ "ten", "desyat" }, 
					new string[]{ "What is this?", "Chto eto takoye?" }, 
					new string[]{ "Open", "Otkryto" }, 
					new string[]{ "Closed", "Zakryto" }, 
					new string[]{ "A little", "Nemnogo" },
					new string[]{ "A lot", "Mnogo" }, 
					new string[]{ "All", "Vse" }, 
					new string[]{ "Breakfast", "Zavtrak" }, 
					new string[]{ "Lunch", "Obed" }, 
					new string[]{ "Dinner", "Uzhin" }, 
					new string[]{ "Cheers!", "Vashe zdorovie!" }
			};
		}
	}
}

