using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Kudu.Views
{
    public partial class WelcomeView : PhoneApplicationPage
    {
        public WelcomeView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(WelcomeView_Loaded);
        }

        void WelcomeView_Loaded(object sender, RoutedEventArgs e)
        {
            // User can only get here either by an initial app load or by having
            // explicitly signing out so make sure they can't signout and then
            // back into a page needs authorization.
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AuthorizationView.xaml", UriKind.Relative));
        }
    }
}