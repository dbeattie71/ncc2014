using Cirrious.MvvmCross.ViewModels;
using DemoApp.Core.Models;
using DemoApp.Core.Services;

namespace DemoApp.Core.ViewModels
{
    public class SpeakerViewModel : MvxViewModel
    {
        public Speaker Speaker
        {
            get { return _speaker; }
            set
            {
                _speaker = value;
                RaisePropertyChanged(() => Speaker);
            }
        }

        public SpeakerViewModel(ICodeCampService codeCampService)
        {
            _codeCampService = codeCampService;
        }

        public void Init(int speakerId)
        {
            Speaker = _codeCampService.GetSpeaker(speakerId);
        }

        private readonly ICodeCampService _codeCampService;
        private Speaker _speaker;
    }
}
