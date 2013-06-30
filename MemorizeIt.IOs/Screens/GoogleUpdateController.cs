using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using System.Threading.Tasks;
using MemorizeIt.MemorySourceSupplier;
using GoogleMemorySupplier;
using System.Drawing;
using System.Linq;

namespace MemorizeIt.IOs.Screens
{
	public class GoogleUpdateController:DialogViewController
	{
		private readonly IMemoryStorage store;
		private readonly ICredentialsStorage credentials;
		private UIBarButtonItem btnLogin;
		private LoadingOverlay loadingOverlay;

		public GoogleUpdateController(IMemoryStorage store,ICredentialsStorage credentials):
			base(UITableViewStyle.Grouped, null)
		{
			
			this.store = store;
			this.credentials = credentials;
			Initialize();
		}

	
		private void ReactOnCredentialsChange(){
			btnLogin.Title = credentials.IsLoggedIn?"Log out":"Log in";
			PopulateSources ();
		}
		protected void Initialize()
		{
			btnLogin =
				new UIBarButtonItem ("",UIBarButtonItemStyle.Plain, (s,e) => Login ());
			this.NavigationItem.SetRightBarButtonItem(btnLogin,false);
			this.NavigationItem.Title="Google Drive Memories";

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ReactOnCredentialsChange ();


		}
		protected void Login(){
			if (credentials.IsLoggedIn) {
				credentials.LogOut ();
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
				credentials.LogIn (dialod.GetTextField(0).Text, dialod.GetTextField (1).Text);
				ReactOnCredentialsChange ();
			};

		}
		protected void Upload(string sourceName)
		{
			if (!credentials.IsLoggedIn)
				return;
			loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			View.Add (loadingOverlay);
		var user = credentials.GetCurrentUser ();
			var supplierParams = new string[]
			{ sourceName, user.Password, user.Password };

			Task.Factory.StartNew (() =>
				                      {
					try
					{
						var supplier = CreateSourceSupplier(supplierParams);
						var data = supplier.Download();
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
			Section items = credentials.IsLoggedIn ? CreateSectionForLoggedIn () : CreateSectionForAnonim ();
			Root = new RootElement ("") {
				items
			};
		}

		protected Section CreateSectionForAnonim ()
		{
			var items = new Section ("Memory Sources ");
			items.Add (new MultilineElement ("Sources are  not avalible, please log in"));
			return items;
		}

		protected Section CreateSectionForLoggedIn ()
		{
			var items = new Section (string.Format ("Memory Sources for {0}", credentials.GetCurrentUser ().Login));
			var listOfSources = CreateSourceSupplier ().GetSourcesList ();
			if (!listOfSources.Any ())
				items.Add (new MultilineElement ("Memory sources are absent"));
			else
				items.AddAll (listOfSources.Select (s => (Element)new StringElement (s, () => Upload (s))));
			return items;
		}

		protected IMemorySourceSupplier CreateSourceSupplier(params string[] supplierParameters)
		{
			return new GoogleMemorySourceSupplier(supplierParameters[0], supplierParameters[1], supplierParameters[2]);
			/*return new SimpleMemorySourceSupplier (new MemoryItem[]{
                new MemoryItem("q1","a1"),
                new MemoryItem("q2","a2"),
                new MemoryItem("q3","a3")
            });*/
		}
		protected IListOfSourcesSupplier CreateSourceSupplier(){
			return new SimpleListOfSourcesSupplier (new String[]{"MemorizeIt"});
		}
	}
}

