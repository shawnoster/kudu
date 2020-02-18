using System.Globalization;
using Kudu.Controls;
using Kudu.ViewModels;

namespace Kudu.Views
{
    /// <summary>
    /// Responsible for changing the shelf a book is currently on as well 
    /// allowing the user to create a new custom shelf.
    /// 
    /// Takes in the id of the book to shelve and does all the work inside the call.
    /// </summary>
    public partial class ShelfPickerView : PickerView
    {
        public ShelfPickerView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            var viewModel = new ShelfPickerViewModel();
            viewModel.Initialize(NavigationContext.QueryString);
            this.DataContext = viewModel;
        }
    }
}