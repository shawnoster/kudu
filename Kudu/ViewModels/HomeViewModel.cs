using System.Windows.Input;
using Kudu.Common;
using Kudu.Services;
using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        /// <summary>
        /// User's currently reading shelf.
        /// </summary>
        public Shelf ReadingShelf
        {
            get
            {
                return GoodreadsClient.Current.GetShelf(
                    GoodreadsClient.Current.AuthorizedUser.Id, 
                    Constants.CurrentlyReadingShelf);
            }
        }

        /// <summary>
        /// List of friend updates.
        /// </summary>
        public FriendUpdateResult FriendUpdateResult
        {
            get
            {
                return GoodreadsClient.Current.GetFriendUpdates();
            }
        }

        /// <summary>
        /// List of shelves.
        /// </summary>
        public ShelfResult ShelfResult
        {
            get
            {
                return GoodreadsClient.Current.GetShelfResult(GoodreadsClient.Current.AuthorizedUser.Id);
            }
        }

        /// <summary>
        /// Selected friend update.
        /// </summary>
        private FriendUpdate _selectedFriendUpdate;
        public FriendUpdate SelectedFriendUpdate
        {
            get
            {
                return _selectedFriendUpdate;
            }
            set
            {
                if (_selectedFriendUpdate != value)
                {
                    _selectedFriendUpdate = value;
                    OnPropertyChanged();

                    //var parameters = new Dictionary<string, string>
                    //{
                    //    {"actorName", SelectedFriendUpdate.Actor.Name},
                    //    {"link", SelectedFriendUpdate.Link}
                    //};

                    //NavigationController.Current.NavigateTo(View.FriendUpdate, parameters);
                }
            }
        }

        //
        // Commands
        //

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new DelegateCommand(
                        p =>
                        {
                            NavigationController.Current.NavigateTo(View.Search);
                        },
                        p =>
                        {
                            return true;
                        }
                        );
                }

                return _searchCommand;
            }
        }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                {
                    _logoutCommand = new DelegateCommand(
                        p =>
                        {
                            // logout by clearing the user id and access tokens and 
                            // navigate the user back to the welcome page.
                            AppSettings.Delete<OAuthAccessToken>(AppSettings.GoodreadsAuth);
                            GoodreadsClient.Current.Deauthenticate();

                            NavigationController.Current.NavigateTo(View.Welcome);
                        });
                }

                return _logoutCommand;
            }
        }
    }
}