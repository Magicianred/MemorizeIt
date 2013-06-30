using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;
using MemorizeIt.MemorySourceSupplier;
using GoogleMemorySupplier;
using FileMemoryStorage;
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
		private UITableView table;
        private readonly IMemoryStorage store;
        private readonly SimpleMemoryTrainer trainer;

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
			this.TabBarItem.Title = "Memories";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			table = new UITableView ();
			View.AddSubview (table);
			
			btnTrain = UIButton.FromType (UIButtonType.RoundedRect);
			btnTrain.SetTitle ("Try me!", UIControlState.Normal);
			btnTrain.TouchUpInside += (sender,e) => Train ();

			View.AddSubview (btnTrain);

			StretchTable ();
			PutButtonAtTheBottomCenter ();

			PopulateTable ();
		}

		public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation (toInterfaceOrientation, duration);
			StretchTable ();
			PutButtonAtTheBottomCenter ();
		}

        protected void Train()
        {
            trainer.PickQuestion();
            ShowQuestion(trainer.CurrentQuestion.Question);
        }

        protected void ShowQuestion(string s)
        {

            var dialod = new UIAlertView("I have a question for you", s, null, "Skip", null);
            dialod.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            dialod.AddButton("Answer");

            dialod.Show();

            dialod.Clicked += (sender, e) =>
                {
                    if (e.ButtonIndex == 0)
                    {
                        trainer.Clear();
                        return;
                    }
                    var answer = dialod.GetTextField(0).Text;
                    var result = trainer.Validate(answer);
                    if (result)
                    {
                        new UIAlertView("Well Done!", string.Format("'{0}' is correct answer", answer), null, "OK", null)
                            .Show();

                    }
                    else
                    {
                        new UIAlertView("Sorry",
                                        string.Format(
                                            "'{0}' is incorrect answer on question '{2}'. Your answer was '{1}'", answer,
                                            trainer.CurrentQuestion.Answer, s), null, "OK", null).Show();
                    }
                    trainer.Clear();
                   
                };

        }

		void PutButtonAtTheBottomCenter ()
		{
			btnTrain.SizeToFit ();
			//btnTrain.Frame.Y = View.Frame.Height - TabBarController.TabBar.Frame.Height - btnTrain.Frame.Height;
			btnTrain.Frame = new RectangleF ((View.Frame.Width - btnTrain.Frame.Width) / 2, 
			                                 GetScreenSize() - btnTrain.Frame.Height, 
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
			return View.Frame.Height - this.NavigationController.NavigationBar.Frame.Height;
		}

        protected void PopulateTable()
        {
            var memories = store.Items;
            if (memories != null)
            {
				var tableSource = new TableSource (memories);

				table.Source = tableSource;
				table.ReloadData ();
				this.NavigationItem.Title = store.GetTableName ();
			}

			if (!trainer.IsQuestionsAvalible ()) {
				new UIAlertView ("Well Done!", "You are done with all your questions", null, "OK", null).Show ();

				btnTrain.Enabled = false;
			} else {
				btnTrain.Enabled = true;
			}
		}

	}
}