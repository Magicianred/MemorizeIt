using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace MemorizeIt.IOs
{
	public static class SimpleAsyncWrapper
	{
		public static void ExecuteAsync(this UIViewController contrller, 
		                                Action action,
		                                Action successHandler, Action<Exception> errorHandler){
			var loadingOverlay = new LoadingOverlay (contrller.View.Frame);
			contrller.View.Add (loadingOverlay);
			Task.Factory.StartNew (() =>
			{
				try {
					action ();
				} catch (Exception exception) {
					contrller.InvokeOnMainThread (() => {
						RemoveLoadingOverlay (loadingOverlay);
						errorHandler(exception);
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

		public static void ExecuteAsync(this UIViewController contrller, 
		                                Action action,
		                                Action successHandler){
			ExecuteAsync (contrller, action,successHandler, (e) => {});
		}

		static void RemoveLoadingOverlay (LoadingOverlay loadingOverlay)
		{
			loadingOverlay.Hide ();
			loadingOverlay.RemoveFromSuperview ();
		}
	}
}

