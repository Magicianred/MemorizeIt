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
using System.Collections.Generic;

namespace MemorizeIt.IOs.Screens
{
	public abstract class GoogleUpdateController:DialogViewController
	{
		protected readonly IMemoryStorage store;
	    protected readonly IMemoryFactory supplier;


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
			this.ExecuteAsync (() => {
				var data = supplier.DownloadMemories (sourceName);
				this.store.Store (data);
			}, () => this.NavigationController.PopToRootViewController (false));
		
		}

		protected virtual void PopulateSources ()
		{
			var listOfSources = new List<string> ();
			this.ExecuteAsync (() => {
				listOfSources = supplier.ListOfSources.ToList ();},
			  
			                   () => {
				Root = CreateSection (listOfSources);}
			);
			
		}

		protected abstract string GetSectionTitle ();
		protected abstract string GetEmptyListReasonTitle ();

		protected virtual void AddElementsInCaseOfEmptyList (Section items)
		{
			items.Add (new MultilineElement (GetEmptyListReasonTitle ()));
		}

		protected RootElement CreateSection (List<string> listOfSources)
		{
			var items = new Section (GetSectionTitle ());
			if (!listOfSources.Any ()) {
				AddElementsInCaseOfEmptyList (items);
				return new RootElement ("") {
					items
				};
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

