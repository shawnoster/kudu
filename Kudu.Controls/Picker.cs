using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Kudu.Controls
{
    public class Picker
    {
        private PhoneApplicationFrame _frame;
        private object _frameContentWhenOpened;
        private NavigationInTransition _savedNavigationInTransition;
        private NavigationOutTransition _savedNavigationOutTransition;
        private PickerView _pickerPage;

        public string Header { get; set; }

        public Uri PickerPageUri { get; set; }

        public object Data { get; set; }

        public void Show()
        {
            OpenPickerPage();
        }

        private void OpenPickerPage()
        {
            if (null == PickerPageUri)
            {
                throw new ArgumentException("PickerPageUri property must not be null.");
            }

            if (null == _frame)
            {
                // Hook up to necessary events and navigate
                _frame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (null != _frame)
                {
                    _frameContentWhenOpened = _frame.Content;

                    // Save and clear host page transitions for the upcoming "popup" navigation
                    UIElement frameContentWhenOpenedAsUIElement = _frameContentWhenOpened as UIElement;
                    if (null != frameContentWhenOpenedAsUIElement)
                    {
                        _savedNavigationInTransition = TransitionService.GetNavigationInTransition(frameContentWhenOpenedAsUIElement);
                        TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUIElement, null);
                        _savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(frameContentWhenOpenedAsUIElement);
                        TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUIElement, null);
                    }

                    _frame.Navigated += OnFrameNavigated;
                    _frame.NavigationStopped += OnFrameNavigationStoppedOrFailed;
                    _frame.NavigationFailed += OnFrameNavigationStoppedOrFailed;

                    _frame.Navigate(PickerPageUri);
                }
            }
        }

        private void ClosePickerPage()
        {            
            if (null == _frame)
            {                
                _frame = Application.Current.RootVisual as PhoneApplicationFrame;

                // Unhook from events
                if (null != _frame)
                {
                    _frame.Navigated -= OnFrameNavigated;
                    _frame.NavigationStopped -= OnFrameNavigationStoppedOrFailed;
                    _frame.NavigationFailed -= OnFrameNavigationStoppedOrFailed;

                    // Restore host page transitions for the completed "popup" navigation
                    UIElement frameContentWhenOpenedAsUIElement = _frameContentWhenOpened as UIElement;

                    if (null != frameContentWhenOpenedAsUIElement)
                    {
                        TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUIElement, _savedNavigationInTransition);
                        _savedNavigationInTransition = null;
                        TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUIElement, _savedNavigationOutTransition);
                        _savedNavigationOutTransition = null;
                    }

                    _frame = null;
                    _frameContentWhenOpened = null;
                }
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _frameContentWhenOpened)
            {
                // Navigation to original page; close the picker page
                ClosePickerPage();
            }
            else if (null == _pickerPage)
            {
                // Navigation to a new page; capture it and push the value in
                _pickerPage = e.Content as PickerView;
                if (null != _pickerPage)
                {
                    _pickerPage.HeaderText = Header;
                    _pickerPage.Data = Data;
                }
            }
        }

        private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e)
        {
            // Abort
            ClosePickerPage();
        }        
    }
}
