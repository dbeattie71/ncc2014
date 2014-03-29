using System.Windows.Controls;
using DemoApp.Core.ViewModels;

namespace DemoApp.Phone.Views
{
    public partial class SessionsView
    {
        public SessionsView()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (e.AddedItems.Count == 1)
            {
                ((SessionsViewModel)ViewModel).Commands["SelectionChanged"].Execute(e.AddedItems[0]);
                ((ListBox) sender).SelectedIndex = -1;
            }
        }
    }
}
