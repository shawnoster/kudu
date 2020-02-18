using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class SearchResultViewModel : ViewModelBase
    {
        public string BookId { get; set; }

        private string _bookTitle;
        public string BookTitle
        {
            get
            {
                return _bookTitle;
            }
            set
            {
                if (value != _bookTitle)
                {
                    _bookTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _author;
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (value != _author)
                {
                    _author = value;
                    OnPropertyChanged();
                }
            }
        }          
    }
}