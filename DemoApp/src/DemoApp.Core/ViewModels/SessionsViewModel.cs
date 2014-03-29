using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using DemoApp.Core.Models;
using DemoApp.Core.Services;

namespace DemoApp.Core.ViewModels
{
    public class SessionsViewModel : BaseViewModel
    {
        public ObservableCollection<Session> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                RaisePropertyChanged(() => Sessions);
            }
        }

        public SessionsViewModel(ICodeCampService codeCampService)
        {
            _codeCampService = codeCampService;
        }

        public void Init()
        {
            Refresh();
        }

        public void SelectionChangedCommand(Session session)
        {
            ShowViewModel<SpeakerViewModel>(new {speakerId = session.SpeakerId});
        }

        private async void Refresh()
        {
            var sessions = await _codeCampService.GetSessions();
            Sessions = new ObservableCollection<Session>(sessions);
        }

        private readonly ICodeCampService _codeCampService;
        private ObservableCollection<Session> _sessions;
    }
}
