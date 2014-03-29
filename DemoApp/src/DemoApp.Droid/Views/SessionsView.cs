using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace DemoApp.Droid.Views
{
    [Activity(Label = "Sessions", MainLauncher = true)]
    public class SessionsView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.sessions_view);
        }
    }
}
