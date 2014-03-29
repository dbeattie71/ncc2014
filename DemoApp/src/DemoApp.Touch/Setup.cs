using MonoTouch.UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using DemoApp.Core.Rest;
using System.Net.Http;
using ModernHttpClient;

namespace DemoApp.Touch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override void InitializePlatformServices ()
		{
			base.InitializePlatformServices ();

			HttpClientFactory.Get = (()=> new HttpClient(new NSUrlSessionHandler()));
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
	}
}