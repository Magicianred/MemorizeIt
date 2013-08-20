using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;
using MemorizeIt.MemorySourceSupplier;

using MemorizeIt.MemoryTrainers;
using MemorizeIt.IOs.ApplicationLayer;
using System.Threading.Tasks;
using MemorizeIt.Model;
using System.Drawing;
using MemorizeIt.IOs.Controls;


namespace MemorizeIt.IOs.Screens {

    /// <summary>
    /// A UITableViewController that uses MonoTouch.Dialog - displays the list of Tasks
    /// </summary>
	public class HomeScreen : UIViewController
    {
     
		private UIButton btnTrain;
		private UIBarButtonItem btnBarUpdate;
		private UITableView table;
        private readonly IMemoryStorage store;
        private readonly SimpleMemoryTrainer trainer;
		private SourceTypeController updateController;

		public HomeScreen(IMemoryStorage store)
			: base()
        {
			this.store = store;
			this.store.SotrageChanged += (e,s) => {
				this.InvokeOnMainThread (PopulateTable);};
			this.trainer = new SimpleMemoryTrainer(this.store);

			Initialize();
        }


        protected void Initialize()
		{	
			table = new UITableView ();

			btnTrain = UIButton.FromType (UIButtonType.RoundedRect);
			btnTrain.SetImage (UIImage.FromFile ("brain-50.png"), UIControlState.Normal);
			btnTrain.SetImage (UIImage.FromFile ("brain-50.png"), UIControlState.Highlighted);
			btnTrain.SetImage (UIImage.FromFile ("brain-50.png"), UIControlState.Disabled);

			btnTrain.TouchUpInside += (sender,e) => Train ();

			btnBarUpdate =
				new UIBarButtonItem ("Update", UIBarButtonItemStyle.Plain, (s,e) => Update ());

		}

		public override void ViewDidAppear (bool animated)
		{
			
			PopulateTable ();
			base.ViewDidAppear (animated);
			
			StretchTable ();		

			PutButtonAtTheBottom ();

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	
			View.AddSubview (table);
			View.AddSubview (btnTrain);

			this.NavigationItem.SetRightBarButtonItem(btnBarUpdate,false);
		}

		public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation (toInterfaceOrientation, duration);
			StretchTable ();
			PutButtonAtTheBottom ();
		}

		protected void Update(){
			if (updateController == null)
				updateController = new SourceTypeController (this.store);
		//	this.ActivateController (updateController);
			this.NavigationController.PushViewController (updateController, true);		
		}

        protected void Train()
        {
            trainer.PickQuestion();
            ShowQuestion(trainer.CurrentQuestion.Question);
        }

        protected void ShowQuestion(string question)
		{

			var dialod = new UIAlertView ("I have a question for you", question, null, "Stop", null);
			dialod.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			dialod.AddButton ("Answer");

			dialod.Show ();

			dialod.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex == 0) {
					trainer.Clear ();
					return;
				}
				var answer = dialod.GetTextField (0).Text;
				var result = trainer.Validate (answer);
				if (result) {                      
					ShowSuccessDialog (answer);
				} else {
					ShowFaliureDialog(answer);
				}                   
			};

		}

		protected void ShowSuccessDialog(string answer){
			var dialog = new UIAlertView ("Well Done!", 
			                              string.Format ("'{0}' is correct answer for '{1}'", 
			               					answer, trainer.CurrentQuestion.Question), 
			                              null, "Stop", null);
			AddNextButtonToDialogAndAssignTrainActionAction (dialog);
			dialog.Show ();
		}

		protected void ShowFaliureDialog(string answer){
			var dialog = new UIAlertView ("Wrong",
			                              string.Format (
				"Correct answer for '{2}' is '{1}'. Your answer was '{0}'", answer,
				trainer.CurrentQuestion.Answer, trainer.CurrentQuestion.Question), null, "Stop", null);
			AddNextButtonToDialogAndAssignTrainActionAction (dialog);
			dialog.Show ();		
		}

		protected void AddNextButtonToDialogAndAssignTrainActionAction(UIAlertView dialog){
			dialog.AddButton ("Next");
			dialog.Clicked += (s,e) => {				
				trainer.Clear ();
				if (e.ButtonIndex == 0) {
					return;
				}
				Train ();
			};
		}

		void PutButtonAtTheBottom ()
		{
			btnTrain.SizeToFit ();
			//btnTrain.Frame.Y = View.Frame.Height - TabBarController.TabBar.Frame.Height - btnTrain.Frame.Height;
			btnTrain.Frame = new RectangleF ((View.Frame.Width - btnTrain.Frame.Width) / 2, 
			                                 GetScreenSize() - btnTrain.Frame.Height - 10 , 
			                                 btnTrain.Frame.Width, btnTrain.Frame.Height);

		}

		void StretchTable ()
		{
			table.Frame = new RectangleF (0, 0, 
			                            View.Frame.Width, 
			                            GetScreenSize ());
		}

		protected float GetScreenSize ()
		{
			//	return View.Frame.Height;
			return View.Frame.Height;// - this.NavigationController.NavigationBar.Frame.Height;
		}

        protected void PopulateTable()
		{
			if (!this.IsViewLoaded)
				return;
			if (store.Empty ()) {
				new UIAlertView ("Memories are empty", "Please upload memories", null, "OK", null).Show ();

				Update ();
				return;
			}

			var tableSource = new TableSource (store.Items);
			table.Source = tableSource;
			table.ReloadData ();

			this.NavigationItem.Title = store.GetTableName ();

			if (!trainer.IsQuestionsAvalible ()) {
				new UIAlertView ("Well Done!", "You are done with all your questions", null, "OK", null).Show ();

				btnTrain.Enabled = false;
			} else {
				btnTrain.Enabled = true;
			}
				
		}

	}
}