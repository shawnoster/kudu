using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using AgFx;
using Kudu.Common;
using Kudu.Services;
using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        private Book _details;
        private UserReview _userReview;

        /// <summary>
        /// Returns the BookId.
        /// </summary>
        public string BookId { get; private set; }

        /// <summary>
        /// Returns true if the book is on any of the users shelves.
        /// </summary>
        private bool _IsOnUsersShelf;
        public bool IsOnUsersShelf
        {
            get
            {
                return _IsOnUsersShelf;
            }
            set
            {
                if (_IsOnUsersShelf != value)
                {
                    _IsOnUsersShelf = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Controls of the visibility of review-related items.
        /// </summary>
        private Visibility _userReviewVisibility = Visibility.Collapsed;
        public Visibility UserReviewVisibility
        {
            get
            {
                return _userReviewVisibility;
            }
            set
            {
                if (_userReviewVisibility != value)
                {
                    _userReviewVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Comma seperated list of shelf names.
        /// </summary>
        public string ShelfNames
        {
            get
            {
                if (IsOnUsersShelf)
                {
                    return string.Join(",", UserReview.Shelves);
                }
                else
                {
                    return "none";
                }
            }
        }

        /// <summary>
        /// Returns the Book details
        /// </summary>
        public Book Book
        {
            get
            {
                if (_details == null)
                {
                    _details = GoodreadsClient.Current.GetBookDetails(BookId);
                }
                return _details;
            }
        }

        /// <summary>
        /// Returns the user's review of the book.
        /// </summary>
        public UserReview UserReview
        {
            get
            {
                if (_userReview == null)
                {
                    _userReview = GoodreadsClient.Current.GetUsersReview(
                        GoodreadsClient.Current.AuthorizedUser.Id,
                        BookId,
                        review =>
                        {
                            SetReviewState(true);
                        },
                        ex =>
                        {
                            // if we get a status code of Not Found that means there
                            // isn't a user review available, any other error is
                            // considered unknown and we reraise.
                            var exceptionHandled = false;
                            if (ex is LoadRequestFailedException)
                            {
                                var requestEx = ex.InnerException as RestRequestException;
                                if (requestEx != null)
                                {
                                    if (requestEx.Status == HttpStatusCode.NotFound)
                                    {
                                        SetReviewState(false);
                                        exceptionHandled = true;
                                    }
                                }
                            }

                            if (!exceptionHandled)
                            {
                                throw ex;
                            }
                        });
                }

                return _userReview;
            }
        }

        #region Commands

        private ICommand _addToShelfCommand;
        public ICommand AddToShelfCommand
        {
            get
            {
                if (_addToShelfCommand == null)
                {
                    _addToShelfCommand = new DelegateCommand(
                        p =>
                        {
                            // ask the user what shelf they'd like to put the book on
                            GoodreadsClient.Current.AddBookToShelf(BookId, Constants.CurrentlyReadingShelf);
                        },
                        p =>
                        {
                            return true;
                        }
                        );
                }

                return _addToShelfCommand;
            }
        }

        private ICommand _updateProgressCommand;
        public ICommand UpdateProgressCommand
        {
            get
            {
                if (_updateProgressCommand == null)
                {
                    _updateProgressCommand = new DelegateCommand(
                        p =>
                        {
                            var parameters = new Dictionary<string, string>
                            {
                                {"id", BookId}
                            };

                            NavigationController.Current.NavigateTo(View.UpdateProgress, parameters);                            
                        });
                }

                return _updateProgressCommand;
            }
        }

        public string ShelfLocalUri
        {
            get
            {
                //ShelfPicker picker = new ShelfPicker();
                //picker.Show();
                return "/Views/ShelfPickerView.xaml?id=" + BookId;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the current Book instance with the values in parameters.
        /// </summary>
        /// <param name="parameters"></param>
        public override void Initialize(IDictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                string id;
                if (!parameters.TryGetValue("id", out id))
                {
                    throw new ArgumentException("A valid book id must be present in the parameters collection", "id");
                }
                BookId = id;

                this.UserReview.PropertyChanged += UserReview_PropertyChanged;
            }
        }

        /// <summary>
        /// Forces a refresh of all the data.
        /// </summary>
        public void RefreshData()
        {
            Book.Refresh();
            UserReview.Refresh();
        }

        private void SetReviewState(bool hasReview)
        {
            IsOnUsersShelf = hasReview;

            if (hasReview)
            {
                UserReviewVisibility = Visibility.Visible;
            }
            else
            {
                UserReviewVisibility = Visibility.Collapsed;
            }
        }

        private void UserReview_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Shelves":
                    OnPropertyChanged("ShelfNames");
                    return;

                default:
                    break;
            }
        }
    }
}
