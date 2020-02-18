using System.Windows;
using Simian.Mvvm;

namespace Kudu.Views
{
    public partial class HomeView : ViewBase
    {
        public HomeView()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(HomeView_Loaded);
        }

        void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            // prevent user from backing into the account authorization page
            // TODO: Make the authorization page aware of being authorized or not
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }
    }
}