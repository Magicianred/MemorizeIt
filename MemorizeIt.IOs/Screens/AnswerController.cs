using System;
using MonoTouch.UIKit;
using MemorizeIt.MemoryTrainers;
using MemorizeIt.MemoryStorage;
using MonoTouch.Foundation;
using System.Drawing;

namespace MemorizeIt.IOs
{
	public class AnswerController: UIViewController
	{		
		private readonly SimpleMemoryTrainer trainer;
		private readonly UITextField tfAnswer;
		private readonly UILabel lblQuestion;
		private readonly int defaultPadding = 20;

		public AnswerController (IMemoryStorage store) : base ()
		{
			this.trainer = new SimpleMemoryTrainer(store);
			tfAnswer = new UITextField ();
			tfAnswer.ReturnKeyType = UIReturnKeyType.Done;
			tfAnswer.BorderStyle = UITextBorderStyle.RoundedRect;
			tfAnswer.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			tfAnswer.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			tfAnswer.AutocorrectionType = UITextAutocorrectionType.No;

			lblQuestion = new UILabel ();
			lblQuestion.TextAlignment = UITextAlignment.Center;
			//lblQuestion.Lines = 0;
			lblQuestion.LineBreakMode = UILineBreakMode.WordWrap;
			lblQuestion.Font = UIFont.BoldSystemFontOfSize (UIFont.LabelFontSize);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			View.BackgroundColor = UIColor.White;

			View.AddSubview (lblQuestion);
			View.AddSubview (tfAnswer);
		
			tfAnswer.EditingDidEndOnExit += tfAnswer_didEndOnExit;
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			tfAnswer.ResignFirstResponder();
			lblQuestion.Text = "";
			trainer.Clear ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewDidAppear (animated);	
			Train ();	
		}

		public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation (toInterfaceOrientation, duration);
			AjustLayoutToScreen ();
		}

		private void AjustLayoutToScreen(){

			var containerWidth = View.Frame.Width - defaultPadding * 2;

			var stringSize =
				new NSString (lblQuestion.Text).StringSize (lblQuestion.Font, new SizeF (containerWidth, 
			                                                                          View.Frame.Height));
			lblQuestion.Frame = new RectangleF (defaultPadding, 
			                                    this.NavigationController.NavigationBar.Frame.Height + defaultPadding + 10,
			                                    containerWidth, stringSize.Height);
			tfAnswer.SizeToFit ();
			tfAnswer.Frame = new RectangleF (defaultPadding, 
			                                 lblQuestion.Frame.Y+lblQuestion.Frame.Height + defaultPadding, 
			                                 containerWidth, tfAnswer.Frame.Height);
		}

		private void tfAnswer_didEndOnExit(object sender, EventArgs e){
			CommitAnswer();
		}

		void CommitAnswer ()
		{
			var answer = tfAnswer.Text;
			var result = trainer.Validate (answer);
			if (result) {
				ShowSuccessDialog (answer);
			}
			else {
				ShowFaliureDialog (answer);
			}
		}
		protected void Train()
		{
			trainer.PickQuestion();
			ShowQuestion(trainer.CurrentQuestion.Question);				
			AjustLayoutToScreen ();
			tfAnswer.BecomeFirstResponder ();
		}

		protected void ShowQuestion(string question)
		{
			lblQuestion.Text = question;
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
				tfAnswer.Text="";
				if (e.ButtonIndex == 0) {				
					NavigationController.PopViewControllerAnimated(true);
					return;
				}
				Train ();
			};
		}
	}
}

