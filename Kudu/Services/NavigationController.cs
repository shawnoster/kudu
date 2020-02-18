using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace Kudu.Services
{
    public enum View
    {
        Startup,
        Home,
        Search,
        Book,
        Settings,
        Authorization,
        Welcome,
        About,
        Notifications,
        UpdateProgress,
        FriendUpdate,
        Shelf
    }

    public class NavigationController
    {
        private static NavigationController _current;
        private Dictionary<View, string> _views;

        public static NavigationController Current
        {
            get
            {
                if (_current == null)
                    _current = new NavigationController();

                return _current;
            }
        }

        public NavigationController()
        {
            _views = new Dictionary<View, string>();            

            //register views with controller
            Register(View.Startup, "/Startup");
            Register(View.Home, "/Views/HomeView.xaml");
            Register(View.Search, "/Views/SearchView.xaml");
            Register(View.Book, "/Views/BookView.xaml");
            Register(View.Settings, "/Views/SettingsView.xaml");
            Register(View.Authorization, "/Views/AuthorizationView.xaml");
            Register(View.Welcome, "/Views/WelcomeView.xaml");
            Register(View.About, "/Kudu.About;component/AboutView.xaml");
            Register(View.Notifications, "/Views/NotificationsView.xaml");
            Register(View.UpdateProgress, "/Views/UpdateProgressView.xaml");
            Register(View.FriendUpdate, "/Views/FriendUpdateView.xaml");
            Register(View.Shelf, "/Views/ShelfView.xaml");
        }

        public void NavigateTo(View view)
        {
            NavigateTo(view, null);
        }

        public void NavigateTo(View view, Dictionary<string, string> parameters)
        {
            if (!Deployment.Current.CheckAccess())
            {
                var context = new DispatcherSynchronizationContext(Deployment.Current.Dispatcher);
                context.Send(_ => NavigateTo(view, parameters), null);
                return;
            }

            if (!_views.ContainsKey(view))
                return;

            string address = _views[view];

            StringBuilder builder = new StringBuilder(address);            
            if (parameters != null && parameters.Count > 0)
            {
                builder.Append("?");
                bool prependAmp = false;
                foreach (KeyValuePair<string, string> parameterPair in parameters)
                {
                    if (prependAmp)
                    {
                        builder.Append("&");
                    }
                    builder.AppendFormat("{0}={1}", parameterPair.Key, parameterPair.Value);
                    prependAmp = true;
                }
            }

            address = builder.ToString();

            PhoneApplicationFrame root = Application.Current.RootVisual as PhoneApplicationFrame;
            Debug.Assert(root != null, "Root is null");
            root.Navigate(new Uri(address, UriKind.Relative));
        }

        public void GoBack()
        {
            if (!Deployment.Current.CheckAccess())
            {
                var context = new DispatcherSynchronizationContext(Deployment.Current.Dispatcher);
                context.Send(_ => GoBack(), null);
                return;
            }

            PhoneApplicationFrame root = Application.Current.RootVisual as PhoneApplicationFrame;
            Debug.Assert(root != null, "Root is null");
            if (root.CanGoBack)
                root.GoBack();
        }

        private void Register(View view, string address)
        {
            if (_views.ContainsKey(view)) //update
            {
                _views[view] = address;
                return;
            }

            _views.Add(view, address);
        }

        private void UnRegister(View type)
        {
            if (_views.ContainsKey(type))
                _views.Remove(type);
        }
    }
}
