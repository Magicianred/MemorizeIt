using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using System.Threading.Tasks;
using MemorizeIt.MemorySourceSupplier;
using GoogleMemorySupplier;
using System.Drawing;

namespace MemorizeIt.IOs.Screens
{
	public class UpdateController:DialogViewController
	{
		private readonly IMemoryStorage store;
		private readonly ICredentialsStorage credentials;
		private UIBarButtonItem btnLogin;
		private LoadingOverlay loadingOverlay;

		public UpdateController(IMemoryStorage store,ICredentialsStorage credentials):
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
		protected void Upload()
		{
			if (!credentials.IsLoggedIn)
				return;
			loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			View.Add (loadingOverlay);
		var user = credentials.GetCurrentUser ();
			var supplierParams = new string[]
			{ "MemorizeIt", user.Password, user.Password };

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
			Section items = new Section ("Memory Sources");
			if (!credentials.IsLoggedIn)

				items.Add (new MultilineElement("Sources are  not avalible, please log in"));
			else {
				items.Add (new StringElement("MemorizeIt",()=> Upload()));
			}

			Root = new RootElement ("") {
				items
			};
		}
		protected IMemorySourceSupplier CreateSourceSupplier(params string[] supplierParameters)
		{
			return new GoogleMemorySourceSupplier(supplierParameters[0], supplierParameters[1], supplierParameters[2]);
			/*return new SimpleMemorySourceSupplier (new MemoryItem[]{
                new MemoryItem("q1","a1"),
                new MemoryItem("q2","a2"),
                new MemoryItem("q3","a3")
            });*/}

	}
}

