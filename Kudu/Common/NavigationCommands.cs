using System.Collections.Generic;
using System.Windows.Input;
using Kudu.Services;
using Librarian;

namespace Kudu.Common
{
    public class NavigationCommands
    {
        private ICommand _viewBook;
        public ICommand ViewBook
        {
            get
            {
                if (_viewBook == null)
                {
                    _viewBook = new DelegateCommand(
                        p =>
                        {
                            var review = p as UserReview;
                            if (review != null)
                            {
                                var parameters = new Dictionary<string, string>
                                {
                                    {"id", review.BookId}
                                };

                                NavigationController.Current.NavigateTo(View.Book, parameters);
                            }
                        });
                }

                return _viewBook;
            }
        }

        private ICommand _viewShelf;
        public ICommand ViewShelf
        {
            get
            {
                if (_viewShelf == null)
                {
                    _viewShelf = new DelegateCommand(
                        p =>
                        {
                            var shelf = p as Shelf;
                            if (shelf != null)
                            {
                                var parameters = new Dictionary<string, string>
                                {
                                    {"shelf", shelf.Name}
                                };

                                NavigationController.Current.NavigateTo(View.Shelf, parameters);
                            }
                        });
                }

                return _viewShelf;
            }
        }
    }
}
