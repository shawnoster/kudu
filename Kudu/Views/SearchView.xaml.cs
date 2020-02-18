using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Kudu.ViewModels;
using Simian.Mvvm;

namespace Kudu.Views
{
    public partial class SearchView : ViewBase
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void SearchQueryText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {                
                this.Focus();                
                (ViewModel as SearchViewModel).Search();
            }
        }

        private void SearchQueryText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }
    }
}