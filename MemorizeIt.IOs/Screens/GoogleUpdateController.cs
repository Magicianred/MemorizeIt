using System;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using System.Threading.Tasks;
using MemorizeIt.MemorySourceSupplier;
//using GoogleMemorySupplier;
using System.Drawing;
using System.Linq;
using MemorizeIt.Model;
using GoogleMemorySupplier;

namespace MemorizeIt.IOs.Screens
{
	public class GoogleUpdateController:DialogViewController
	{
		private readonly IMemoryStorage store;
	    private readonly IMemoryFactory supplier;
		private UIBarButtonItem btnLogin;
		private LoadingOverlay loadingOverlay;

		public GoogleUpdateController(IMemoryStorage store):
			base(UITableViewStyle.Grouped, null)
		{
			
			this.store = store;
			this.supplier=new GoogleMemoryFactory();
			Initialize();
		}

	
		private void ReactOnCredentialsChange(){
			btnLogin.Title = supplier.CredentialsStorage.IsLoggedIn?"Log out":"Log in";
			PopulateSources ();
		}
		protected void Initialize()
		{
			btnLogin =
				new UIBarButtonItem ("",UIBarButtonItemStyle.Plain, (s,e) => Login ());
			this.NavigationItem.SetRightBarButtonItem(btnLogin,false);
			this.NavigationItem.Title="Google Drive Memories";

			this.NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem ("Back", UIBarButtonItemStyle.Done, (s,e) => {

				NavigationController.PopViewControllerAnimated(true);
			}), false);

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ReactOnCredentialsChange ();


		}
		protected void Login(){
            if (supplier.CredentialsStorage.IsLoggedIn)
            {
                supplier.CredentialsStorage.LogOut();
				ReactOnCredentialsChange ();
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
				try {
                    supplier.CredentialsStorage.LogIn(dialod.GetTextField(0).Text, dialod.GetTextField(1).Text);
					ReactOnCredentialsChange ();
				} catch (CredentialsException ex) {
					this.InvokeOnMainThread(() =>
					                        new UIAlertView("Error", ex.Message, null, "OK",
					                null).Show());
				}
			};

		}
		protected void Upload(string sourceName)
		{
			loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			View.Add (loadingOverlay);
			Task.Factory.StartNew (() =>
				                      {
					try
					{
					    var data = supplier.DownloadMemories(sourceName);
						this.store.Store(data);
					}
					catch (Exception downloadException)
					{
						this.InvokeOnMainThread(() =>
						                        new UIAlertView("Error", downloadException.Message, null, "OK",
						                null).Show());
					}
					this.InvokeOnMainThread(() =>
					                        loadingOverlay.Hide());
				});

		}

		void PopulateSources ()
		{
			Root = supplier.CredentialsStorage.IsLoggedIn ? CreateSectionForLoggedIn() : CreateSectionForAnonim();

		}

		protected RootElement CreateSectionForAnonim ()
		{
			var items = new Section ("Memory Sources ");
			items.Add (new MultilineElement ("Sources are  not avalible, please log in"));
			return new RootElement ("") { items };
		}

		protected RootElement CreateSectionForLoggedIn ()
		{
			var items = new Section (string.Format ("Memory Sources for {0}", supplier.CredentialsStorage.GetCurrentUser ().Login));
		    var listOfSources = supplier.ListOfSources.ToList();
			if (!listOfSources.Any ()) {
				items.Add (new MultilineElement ("Memory sources are absent"));
				return new RootElement ("") { items };
			}
			var rGroup = new RadioGroup (-1);
			foreach (var item in listOfSources) {
				var radioButton = new RadioElement (item);
				radioButton.Tapped += () => Upload (listOfSources[rGroup.Selected]);
				items.Add (radioButton);
			}
			return new RootElement ("", rGroup) { items };

		}

	}
}

