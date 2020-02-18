using System.Windows.Controls;
using System.Windows.Data;
using Simian.Mvvm;

namespace Kudu.Views
{
    public partial class UpdateProgressView : ViewBase
    {
        public UpdateProgressView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }
    }
}