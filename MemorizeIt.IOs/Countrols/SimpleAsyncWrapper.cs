using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace MemorizeIt.IOs
{
	public static class SimpleAsyncWrapper
	{
		public static void ExecuteAsync(this UIViewController contrller, 
		                                Action action,
		                                Action successHandler){
			var loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			contrller.View.Add (loadingOverlay);
			Task.Factory.StartNew (() =>
			{
				try {
					action ();
				} catch (Exception exception) {
					contrller.InvokeOnMainThread (() => {
						RemoveLoadingOverlay (loadingOverlay);
						new UIAlertView ("Error", exception.Message, null, "OK",
						                 null).Show ();
					});
				}
				contrller.InvokeOnMainThread (() => {
					RemoveLoadingOverlay (loadingOverlay);
					successHandler ();
				}
				);
			});
		}

		static void RemoveLoadingOverlay (LoadingOverlay loadingOverlay)
		{
			loadingOverlay.Hide ();
			loadingOverlay.RemoveFromSuperview ();
		}
	}
}

