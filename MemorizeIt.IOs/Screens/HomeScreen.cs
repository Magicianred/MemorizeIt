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

        public HomeScreen()
            : base()
        {
			this.store = new FileSystemMemoryStorage();
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


			table = new UITableView (new RectangleF (0, 0, View.Frame.Width
			                                                     , View.Frame.Height - TabBarController.TabBar.Frame.Height));
			View.AddSubview (table);
			
			btnTrain = UIButton.FromType (UIButtonType.RoundedRect);
			btnTrain.SetTitle ("Train", UIControlState.Normal);
			btnTrain.TouchUpInside += (sender,e) => Train ();

			View.AddSubview (btnTrain);
			btnTrain.SizeToFit ();

			PopulateTable ();
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
                    PopulateTable();

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
        protected void PopulateTable()
        {
            var memories = store.Items;
            if (memories != null)
            {
				var tableSource = new TableSource (memories);

				table.Source = tableSource;
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