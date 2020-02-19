using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;
using AgFx;
using Kudu.Common;
using Librarian;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Kudu
{
    public partial class App : Application
    {
        // API keys have been revoked, not a security risk but also
        // won't work without fetching a new dev token
        private static string ConsumerKey = "t6jiT5Rr6zXqL450555b6Q";
        private static string ConsumerSecret = "BjNOEop1UxEiYtuupYe4zO1zeW9pIb5SV3qfV6zYE";

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
    
                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                //PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            DataManager.Current.UnhandledError += (s, ex) =>
            {
                var userMessage = ex.ExceptionObject.Message + "\n\n" + ex.ExceptionObject.InnerException;

                // if there is an inner exception get more details
                if (ex.ExceptionObject.InnerException != null)
                {
                    var restException = ex.ExceptionObject.InnerException as RestRequestException;
                    if (restException != null)
                    {
                        XDocument doc = XDocument.Parse(restException.Content);
                        var restError = (from error in doc.Descendants("error")
                                            select error.Value.ToString()).FirstOrDefault();

                        userMessage = restError;
                    }
                }

                Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(userMessage, "DataManager Error", MessageBoxButton.OK); });
                ex.Handled = true;
            };
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            // Singletons
            InitializeGoodreads();
            InitializeUriMapper();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Singletons
            InitializeGoodreads();
            InitializeUriMapper();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Initializes Goodreads client library
        private void InitializeGoodreads()
        {
            GoodreadsClient.Initialize(ConsumerKey, ConsumerSecret);

            OAuthAccessToken accessToken = AppSettings.Load<OAuthAccessToken>(AppSettings.GoodreadsAuth);
            if (accessToken != null)
            {
                if (!String.IsNullOrEmpty(accessToken.Token) && !String.IsNullOrEmpty(accessToken.TokenSecret))
                {
                    GoodreadsClient.Current.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                    GoodreadsClient.Current.AuthenticateUser();
                }
            }
        }

        // Initializes UriMapper used to map to various pages based on application state
        private void InitializeUriMapper()
        {
            UriMapper mapper = Resources["mapper"] as UriMapper;
            RootFrame.UriMapper = mapper;

            if (!String.IsNullOrEmpty(GoodreadsClient.Current.AccessToken))
            {
                mapper.UriMappings[0].MappedUri = new Uri("/Views/HomeView.xaml", UriKind.Relative);
            }
            else
            {
                mapper.UriMappings[0].MappedUri = new Uri("/Views/WelcomeView.xaml", UriKind.Relative);
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
