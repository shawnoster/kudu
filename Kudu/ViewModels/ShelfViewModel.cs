using System;
using System.Collections.Generic;
using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class ShelfViewModel : ViewModelBase
    {
        /// <summary>
        /// Returns the UserId.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Returns the ShelfName.
        /// </summary>
        public string ShelfName { get; private set; }

        /// <summary>
        /// Shelf
        /// </summary>
        public Shelf Shelf
        {
            get
            {
                return GoodreadsClient.Current.GetShelf(UserId, ShelfName);
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
                // user id
                string id;
                if (!parameters.TryGetValue("id", out id))
                {
                    id = GoodreadsClient.Current.AuthorizedUser.Id;
                }
                UserId = id;

                // shelf name
                string shelfName;
                if (!parameters.TryGetValue("shelf", out shelfName))
                {
                    throw new ArgumentException("A valid shelf name must be present in the parameters collection", "shelf");
                }
                ShelfName = shelfName;
            }
        }
    }
}
