using System;
using MemorizeIt.DictionarySourceSupplier.CredentialsStorage;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MemorizeIt.DictionaryStorage;
using System.Threading.Tasks;
using MemorizeIt.DictionarySourceSupplier;
//using GoogleMemorySupplier;
using System.Drawing;
using System.Linq;
using MemorizeIt.Model;
using System.Collections.Generic;

namespace MemorizeIt.IOs.Screens
{
	public abstract class GoogleUpdateController:DialogViewController
	{
		protected readonly IDictionaryStorage store;
	    protected readonly IDictionaryFactory supplier;


		public GoogleUpdateController(IDictionaryStorage store):
			base(UITableViewStyle.Grouped, null,true)
		{
			this.store = store;
			this.supplier = CreateSupplier ();
			Initialize ();
		}
		protected abstract IDictionaryFactory CreateSupplier ();

		protected virtual void Initialize()
		{
		}

		/*public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			PopulateSources ();
		}*/

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
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
				OnSourcesRecivedSuccefully (listOfSources);}, OnSourcesRecivedUnsuccefully
			);
		}

		protected abstract string GetSectionTitle ();
		protected abstract string GetEmptyListReasonTitle ();

		protected virtual void OnSourcesRecivedSuccefully(List<string> listOfSources){			
			Root = CreateSection (listOfSources);
		}
		protected virtual void OnSourcesRecivedUnsuccefully(Exception e){
		}

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

