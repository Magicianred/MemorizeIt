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
			this.NavigationItem.Title="My Google Drive Memories";
		}

		protected override void PopulateSources ()
		{
			base.PopulateSources ();
			btnLogin.Title = supplier.CredentialsStorage.IsLoggedIn?"Log out":"Log in";
		}

		protected void Login(){

			if (supplier.CredentialsStorage.IsLoggedIn) {
				supplier.CredentialsStorage.LogOut ();
				PopulateSources ();
				return;
			}


			var dialod = new UIAlertView ("Enter credentials", "", null, "Cancel", null);

			dialod.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
			dialod.GetTextField (0).KeyboardType = UIKeyboardType.EmailAddress;

			dialod.AddButton ("Log in");

			dialod.Show ();

			dialod.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex == 0)
					return;
				var userName=dialod.GetTextField (0).Text;
				var password=dialod.GetTextField (1).Text;
				this.ExecuteAsync (() => {

					supplier.CredentialsStorage.LogIn (userName,password);
					
				}, PopulateSources);
			};

		}

		protected override string GetSectionTitle ()
		{
			if (supplier.CredentialsStorage.IsLoggedIn)
				return string.Format ("Memory Sources for {0}", supplier.CredentialsStorage.GetCurrentUser ().Login);
			return "Memory Sources";
		}

		protected override void AddElementsInCaseOfEmptyList (MonoTouch.Dialog.Section items)
		{
			base.AddElementsInCaseOfEmptyList (items);
			if (!supplier.CredentialsStorage.IsLoggedIn)
				return;

			var createTemplateSuggestion = new MultilineElement ("But I'll create template for you if you click me");
			createTemplateSuggestion.Tapped += () => {
				this.ExecuteAsync (() => supplier.CreateTemplate (),
				                   ()=>{ 
					PopulateSources();
					var dialod = new UIAlertView ("Done", "Spreadsheet with name MemorizeIt was created at your google drive", null, "I got it!", null);
					dialod.Show();
				});
			};
			items.Add (createTemplateSuggestion);
		}

		protected override string GetEmptyListReasonTitle ()
		{
			if (supplier.CredentialsStorage.IsLoggedIn)
				return "Memory sources are absent";
			return "Sources are  not avalible, please log in";
		}
	}
}

