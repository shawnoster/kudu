using System;
using Kudu.Services;
using Kudu.ViewModels;
using Simian.Mvvm;

namespace Kudu
{
    public partial class ShelfView : ViewBase
    {
        // Constructor
        public ShelfView()
        {
            InitializeComponent();            
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationController.Current.NavigateTo(View.Search);
        }
    }
}