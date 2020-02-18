using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Resources;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Kudu.About
{
    public partial class AboutView : PhoneApplicationPage
    {
        private StackPanel _licenses;

        public AboutView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UpdateVersion();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot piv = (Pivot)sender;
            if (piv.SelectedIndex > 0 && _licenses == null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    _licenses = new StackPanel();

                    StreamResourceInfo sri = Application.GetResourceStream(new Uri("LICENSE.txt", UriKind.Relative));
                    if (sri != null)
                    {
                        using (StreamReader sr = new StreamReader(sri.Stream))
                        {
                            string line;
                            bool lastWasEmpty = true;
                            do
                            {
                                line = sr.ReadLine();

                                if (String.IsNullOrEmpty(line))
                                {
                                    Rectangle r = new Rectangle
                                    {
                                        Height = 20,
                                    };
                                    _licenses.Children.Add(r);
                                    lastWasEmpty = true;
                                }
                                else
                                {
                                    TextBlock tb = new TextBlock
                                    {
                                        TextWrapping = TextWrapping.Wrap,
                                        Text = line,
                                        Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"],
                                    };
                                    if (!lastWasEmpty)
                                    {
                                        tb.Opacity = 0.7;
                                    }
                                    lastWasEmpty = false;
                                    _licenses.Children.Add(tb);
                                }
                            } while (line != null);
                        }
                    }

                    sv1.Content = _licenses;
                });
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string s = ((ButtonBase)sender).Tag as string;

            switch (s)
            {
                case "Review":
                    var task = new MarketplaceReviewTask();
                    task.Show();
                    break;
            }
        }

        private void UpdateVersion()
        {
            _versionText.Text = Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0];
        }
    }
}