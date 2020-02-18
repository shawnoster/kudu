using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls;

namespace Kudu.Controls
{
    public abstract class PickerView : PhoneApplicationPage
    {
        private const string VisibilityGroupName = "VisibilityStates";
        private const string OpenVisibilityStateName = "Open";
        private const string ClosedVisibilityStateName = "Closed";
        private const string StateKey_Value = "PickerPage_State_Value";

        private Storyboard _closedStoryboard;

        /// <summary>
        /// Gets or sets the string of text to display as the header of the page.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the data object that the picker is representing
        /// </summary>
        public object Data { get; set; }

        private void OnDoneButtonClick(object sender, EventArgs e)
        {
            // Commit the value and close            
            //TODO: trigger commit code

            ClosePickerPage();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            // Close without committing a value
            //TODO: trigger value clearing code

            ClosePickerPage();
        }

        /// <summary>
        /// Called when the Back key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            // Cancel back action so we can play the Close state animation (then go back)
            e.Cancel = true;
            ClosePickerPage();
        }

        private void ClosePickerPage()
        {
            // Play the Close state (if available)
            if (null != _closedStoryboard)
            {
                VisualStateManager.GoToState(this, ClosedVisibilityStateName, true);
            }
            else
            {
                OnClosedStoryboardCompleted(null, null);
            }
        }

        private void OnClosedStoryboardCompleted(object sender, EventArgs e)
        {
            // Close the picker page
            NavigationService.GoBack();
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            base.OnNavigatedTo(e);

            // Restore Value if returning to application (to avoid inconsistent state)
            if (State.ContainsKey(StateKey_Value))
            {
                Data = State[StateKey_Value];

                // Back out from picker page for consistency with behavior of core pickers in this scenario
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }

            InitializePickerControls();
            InitializeStoryboards();
            InitializeApplicationBar();

            // Play the Open state
            VisualStateManager.GoToState(this, OpenVisibilityStateName, true);
        }

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            base.OnNavigatedFrom(e);

            // Save Value if navigating away from application
            if ("app://external/" == e.Uri.ToString())
            {
                State[StateKey_Value] = Data;
            }
        }

        protected virtual void InitializePickerControls()
        {
            // override
        }

        private void InitializeStoryboards()
        {
            // Hook up to storyboard(s)
            FrameworkElement templateRoot = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            if (null != templateRoot)
            {
                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(templateRoot))
                {
                    if (VisibilityGroupName == group.Name)
                    {
                        foreach (VisualState state in group.States)
                        {
                            if ((ClosedVisibilityStateName == state.Name) && (null != state.Storyboard))
                            {
                                _closedStoryboard = state.Storyboard;
                                _closedStoryboard.Completed += OnClosedStoryboardCompleted;
                            }
                        }
                    }
                }
            }
        }

        private void InitializeApplicationBar()
        {
            // Customize the ApplicationBar Buttons by providing the right text
            if (null != ApplicationBar)
            {
                foreach (object obj in ApplicationBar.Buttons)
                {
                    IApplicationBarIconButton button = obj as IApplicationBarIconButton;
                    if (null != button)
                    {
                        if ("DONE" == button.Text)
                        {
                            button.Text = Microsoft.Phone.Controls.LocalizedResources.ControlResources.DateTimePickerDoneText;
                            button.Click += OnDoneButtonClick;
                        }
                        else if ("CANCEL" == button.Text)
                        {
                            button.Text = Microsoft.Phone.Controls.LocalizedResources.ControlResources.DateTimePickerCancelText;
                            button.Click += OnCancelButtonClick;
                        }
                    }
                }
            }
        }
    }
}