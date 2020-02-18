using Kudu.Common;
using Kudu.Services;
using Kudu.ViewModels;
using Librarian;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;

namespace Kudu.Views
{
    public partial class AuthorizationView : PhoneApplicationPage
    {
        private const string _callbackUrl = "http://shawnoster.com";

        private AuthorizationViewModel _viewModel;
        private OAuthRequestToken _requestToken;        

        public AuthorizationView()
        {
            InitializeComponent();

            _viewModel = new AuthorizationViewModel();
            _viewModel.IsBusy = true;
            _viewModel.BusyMessage = "Loading...";

            DataContext = _viewModel;

            Browser.Navigated += Browser_Navigated;
            Browser.Navigating += Browser_Navigating;

            Loaded += GoodreadsAuthorizationView_Loaded;
        }

        void GoodreadsAuthorizationView_Loaded(object sender, RoutedEventArgs e)
        {
            // ensure the user is logged out before doing anything fancy.
            //Browser.Navigate(new Uri("http://www.goodreads.com/user/sign_out", UriKind.Absolute));
            //Browser.Navigate(new Uri("http://www.goodreads.com/oauth/authorize?mobile=1", UriKind.Absolute));
            RequestToken();
        }

        void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _viewModel.IsBusy = false;
            Browser.Visibility = Visibility.Visible;
            Browser.Navigated -= Browser_Navigated;
        }

        void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Contains("http://www.goodreads.com/m/home"))
            {
                RequestToken();
            }

            if (!e.Uri.AbsoluteUri.Contains(_callbackUrl))
                return;

            e.Cancel = true;

            // The browser will show an ugly empty gray navigation failure
            // because we canceled the navigation above.  Hide the browser
            // while we finish authenticating.
            _viewModel.IsBusy = true;
            _viewModel.BusyMessage = "Authenticating...";
            Browser.Visibility = Visibility.Collapsed;

            var values = Helpers.ParseQueryString(e.Uri.Query);
            var token = values["oauth_token"];
            if (token != _requestToken.Token)
            {
                throw new Exception("Tokens don't match");
            }

            string verifier;
            values.TryGetValue("oauth_verifier", out verifier);
            GoodreadsClient.Current.GetAccessToken(
                _requestToken,
                verifier,
                accessToken =>
                {
                    // Save settings
                    AppSettings.Save(AppSettings.GoodreadsAuth, accessToken);

                    // At this point we're fully authenticated.  Authenticate the
                    // current client and get the UserId & name while we're at it.
                    GoodreadsClient.Current.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                    GoodreadsClient.Current.AuthenticateUser(
                        user =>
                        {
                            // don't navigate away until the Authorized user info has 
                            // been set, every other page needs the UserId so we'll just
                            // park here until it's set.                    
                            Debug.Assert(user.Id != null);
                            NavigationController.Current.NavigateTo(View.Home);
                        },
                        error =>
                        {
                            //TODO: Better error handling here.
                            MessageBox.Show("Hmm, something has gone horribly wrong");
                        });  
                });
        }

        private void RequestToken()
        {
            GoodreadsClient.Current.GetRequestToken(
                _callbackUrl,
                token =>
                {
                    _requestToken = token;
                    Dispatcher.BeginInvoke(() => Browser.Navigate(GoodreadsClient.GetAuthorizationUri(_requestToken)));
                });
        }
    }
}