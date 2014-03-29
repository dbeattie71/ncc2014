using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace DemoApp.Droid.Views
{
    [Activity(Label = "Speaker")]
    public class SpeakerView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.speaker_view);
        }
    }
}
