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
	public abstract class GoogleUpdateController:DialogViewController
	{
		protected readonly IMemoryStorage store;
	    protected readonly IMemoryFactory supplier;
		private LoadingOverlay loadingOverlay;

		public GoogleUpdateController(IMemoryStorage store):
			base(UITableViewStyle.Grouped, null,true)
		{
			
			this.store = store;
			this.supplier = CreateSupplier ();
			Initialize ();
		}
		protected abstract IMemoryFactory CreateSupplier ();
		protected virtual void Initialize()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			PopulateSources ();
		}

		protected void Upload(string sourceName)
		{
			loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			View.Add (loadingOverlay);
			Task.Factory.StartNew (() =>
			{
				try {
					var data = supplier.DownloadMemories (sourceName);
					this.store.Store (data);
				} catch (Exception downloadException) {
					this.InvokeOnMainThread (() =>
						                        new UIAlertView ("Error", downloadException.Message, null, "OK",
					                                         null).Show ());
				}
				this.InvokeOnMainThread (() =>
					                        loadingOverlay.Hide ());
			});

		}

		protected virtual void PopulateSources ()
		{
			Root = CreateSection ();
		}

		protected abstract string GetSectionTitle ();
		protected abstract string GetEmptyListReasonTitle ();

		protected RootElement CreateSection ()
		{
			var items = new Section (GetSectionTitle ());
		    var listOfSources = supplier.ListOfSources.ToList();
			if (!listOfSources.Any ()) {
				items.Add (new MultilineElement (GetEmptyListReasonTitle()));
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

