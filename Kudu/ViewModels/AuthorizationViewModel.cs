using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get
            {
                return _busyMessage;
            }
            set
            {
                if (value != _busyMessage)
                {
                    _busyMessage = value;
                    OnPropertyChanged();
                }
            }
        }
        
    }
}
