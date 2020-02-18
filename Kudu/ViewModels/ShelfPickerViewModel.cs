using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class ShelfPickerViewModel : ViewModelBase
    {
        /// <summary>
        /// Id of the book being updated.
        /// </summary>
        public string BookId { get; private set; }

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
        /// Returns the user's review of the book.
        /// </summary>
        public UserReview UserReview
        {
            get
            {
                return GoodreadsClient.Current.GetUsersReview(BookId);
            }
        }

        public ReadOnlyCollection<SelectedItemViewModel> ExclusiveShelves
        {
            get
            {
                var _selectedExclusiveShelves = new ObservableCollection<SelectedItemViewModel>();
                foreach (var shelf in ShelfResult.ExclusiveShelves)
                {
                    _selectedExclusiveShelves.Add(new SelectedItemViewModel { DisplayName = shelf.Description, IsChecked = false });
                }

                return new ReadOnlyCollection<SelectedItemViewModel>(_selectedExclusiveShelves);
            }
        }

        public ReadOnlyCollection<Shelf> UserShelves
        {
            get
            {
                return ShelfResult.UserShelves;
            }
        }

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
            }
        }
    }
}
