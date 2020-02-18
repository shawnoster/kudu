using System;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class EditReviewViewModel : ViewModelBase
    {
        private string _bookId;

        private string _review;
        public string Review
        {
            get
            {
                return _review;
            }
            set
            {
                if (_review != value)
                {
                    _review = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _readAt;
        public DateTime ReadAt
        {
            get
            {
                return _readAt;
            }
            set
            {
                if (_readAt != value)
                {
                    _readAt = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _shelf;
        public string Shelf
        {
            get
            {
                return _shelf;
            }
            set
            {
                if (_shelf != value)
                {
                    _shelf = value;
                    OnPropertyChanged();
                }
            }
        }

        public override void Initialize(System.Collections.Generic.IDictionary<string, string> parameters)
        {
            // Book ID
            if (!parameters.TryGetValue("id", out _bookId))
            {
                throw new ArgumentException("Missing book id in parameter list.", "id");
            }
        }
    }
}
