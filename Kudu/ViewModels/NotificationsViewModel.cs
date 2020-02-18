using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        public NotificationResult NotificationResult
        {
            get
            {
                return GoodreadsClient.Current.GetNotifications();
            }
        }
    }
}
