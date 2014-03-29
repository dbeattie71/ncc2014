using Cirrious.MvvmCross.ViewModels;

namespace DemoApp.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public IMvxCommandCollection Commands { get; private set; }

        public BaseViewModel()
        {
            Commands = new MvxCommandCollectionBuilder().BuildCollectionFor(this);
        }
    }
}
