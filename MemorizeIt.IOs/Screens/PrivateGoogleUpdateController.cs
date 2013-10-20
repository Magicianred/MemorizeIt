using System;
using MemorizeIt.MemoryStorage;
using MonoTouch.UIKit;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using GoogleMemorySupplier;
using MemorizeIt.MemorySourceSupplier;
using MonoTouch.Dialog;

namespace MemorizeIt.IOs.Screens
{
	public class PrivateGoogleUpdateController:GoogleUpdateController
	{
		private UIBarButtonItem btnLogin;

		public PrivateGoogleUpdateController (IMemoryStorage store):base(store)
		{
		}

		protected override IMemoryFactory CreateSupplier (){
			return new GoogleMemoryFactory ();
		}
		protected override void Initialize ()
		{
			base.Initialize ();
			btnLogin =
				new UIBarButtonItem ("",UIBarButtonItemStyle.Plain, (s,e) => Login ());
			this.NavigationItem.SetRightBarButtonItem(btnLogin,false);
			this.NavigationItem.Title="My Google Drive";
		}

		protected override void PopulateSources ()
		{
			btnLogin.Enabled = false;
			base.PopulateSources ();
			btnLogin.Title = supplier.CredentialsStorage.IsLoggedIn?"Logout":"Login";

		}

		protected override void OnSourcesRecivedSuccefully (System.Collections.Generic.List<string> listOfSources)
		{
			base.OnSourcesRecivedSuccefully (listOfSources);
			btnLogin.Enabled = true;
		}
		protected override void OnSourcesRecivedUnsuccefully (Exception e)
		{
			base.OnSourcesRecivedUnsuccefully (e);
			btnLogin.Enabled = true;
		}

		protected void Login(){

			if (supplier.CredentialsStorage.IsLoggedIn) {
				supplier.CredentialsStorage.LogOut ();
				PopulateSources ();
				return;
			}


			var dialod = new UIAlertView ("Enter Google Drive credentials", "", null, "Cancel", null);

			dialod.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
			dialod.GetTextField (0).KeyboardType = UIKeyboardType.EmailAddress;

			dialod.AddButton ("Login");

			dialod.Show ();

			dialod.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex == 0)
					return;
				var userName = dialod.GetTextField (0).Text;
				var password = dialod.GetTextField (1).Text;
				this.ExecuteAsync (() => {
					
					btnLogin.Enabled = false;
					supplier.CredentialsStorage.LogIn (userName, password);
					
				}, PopulateSources);
			};

		}

		protected override string GetSectionTitle ()
		{
			if (supplier.CredentialsStorage.IsLoggedIn)
				return string.Format ("Memory Sources for {0}", supplier.CredentialsStorage.GetCurrentUser ().Login);
			return "My Memory Sources";
		}

		protected override void AddElementsInCaseOfEmptyList (MonoTouch.Dialog.Section items)
		{
			if (!supplier.CredentialsStorage.IsLoggedIn) {				
				base.AddElementsInCaseOfEmptyList (items);
				return;
			}

			var createTemplateSuggestion = new MultilineElement ("Sources was't found. Tap to create source spreadsheet at your Google Drive ");

			createTemplateSuggestion.Tapped += () => {
				this.ExecuteAsync (supplier.CreateTemplate,
				() => { 
					PopulateSources ();
					var dialod = new UIAlertView ("Done", "Spreadsheet with name MemorizeIt was created at your Google Drive", null, "I got it!", null);
					dialod.Show ();
				});
			};

			items.Add (createTemplateSuggestion);
		}

		protected override string GetEmptyListReasonTitle ()
		{
			return "Please login to see your sources";
		}
	}
}

