using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;
using MemorizeIt.MemorySourceSupplier;
using GoogleMemorySupplier;
using FileMemoryStorage;
using MemorizeIt.MemoryTrainers;
using MemorizeIt.IOs.ApplicationLayer;
namespace MemorizeIt.IOs.Screens {

	/// <summary>
	/// A UITableViewController that uses MonoTouch.Dialog - displays the list of Tasks
	/// </summary>
	public class HomeScreen : DialogViewController {
		// 

		
		// MonoTouch.Dialog individual TaskDetails view (uses /AL/TaskDialog.cs wrapper class)
		BindingContext context;
		DialogViewController detailsScreen;
		private readonly IMemoryStorage store;
		private readonly SimpleMemoryTrainer trainer;
		public HomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
			this.store=new FileSystemMemoryStorage();
			this.trainer = new SimpleMemoryTrainer (this.store);
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem ("Upadate",

			    UIBarButtonItemStyle.Plain,
			                     (sender, e) => { Upload(); }), false);
			NavigationItem.SetLeftBarButtonItem (
				new UIBarButtonItem ("Train",

			                      UIBarButtonItemStyle.Plain,
			                      (sender, e) => {
				Train (); }), false);
		}
		protected void Upload(){

			var dialod =new UIAlertView("Enter credentials", "Document path",null, "Cancel", null);

			dialod.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
			dialod.AddButton ("Upload");

			dialod.Show ();

			dialod.Clicked += (sender,e) => {
				if(e.ButtonIndex==0)
					return;
				var supplier = new GoogleMemorySourceSupplier ("MemorizeIt", 
				                                               dialod.GetTextField(0).Text, 
				                                               dialod.GetTextField(1).Text);
				var data = supplier.Download ();
				this.store.Store (data);
				PopulateTable ();
			};


		}

		protected void Train(){
			ShowQuestion (trainer.GetQuestion());

		}

		protected void ShowQuestion(string s){

			var dialod =new UIAlertView("I have a question for you", s,null, "Skip", null);

			dialod.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			dialod.AddButton ("Answer");

			dialod.Show ();

			dialod.Clicked += (sender,e) => {
				if (e.ButtonIndex == 0) {
					trainer.Clear ();
					return;
				}
				trainer.Validate (dialod.GetTextField (0).Text);
				PopulateTable ();
			};

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();			
		}
		
		protected void PopulateTable()
		{
			var memories = store.Items;
			var items = new Section () {

				from t in memories
				select (Element) new StringElement(string.Format("{1}({0})",t.SuccessCount,t.Values[0]),t.Values[1])
			};

			Root = new RootElement ("Memories") {
				items
			};
		}

		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}

	}
}