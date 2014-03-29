using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using DemoApp.Core.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace DemoApp.Touch.Views
{
    [Register("SessionsView")]
    public class SessionsView : MvxTableViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // ios7 layout
//            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
//                EdgesForExtendedLayout = UIRectEdge.None;

            var source = new MvxStandardTableViewSource(TableView, "TitleText Name;");
            TableView.Source = source;

            var set = this.CreateBindingSet<SessionsView, SessionsViewModel>();
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.Commands["SelectionChanged"]);
            set.Bind(source).To(vm => vm.Sessions);
            set.Apply();

            TableView.ReloadData();
        }
    }
}
