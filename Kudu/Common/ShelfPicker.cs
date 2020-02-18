using System;
using Kudu.Controls;

namespace Kudu.Common
{
    public class ShelfPicker : Picker
    {
        public ShelfPicker()
        {
            this.Header = "Shelf";
            this.PickerPageUri = new Uri("/Views/ShelfPickerView.xaml", UriKind.Relative);
        }
    }
}
