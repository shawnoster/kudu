using System;
using AgFx;

namespace Librarian
{
    public class FriendUpdate : ModelItemBase
    {
        /// <summary>
        /// The action text for the update, included HTML and hyperlinks to 
        /// the Goodreads website.
        /// </summary>
        public string ActionText { get; set; }

        /// <summary>
        /// A link to the update on the Goodreads update.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// URL to the image associated with this update.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Date the user updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        private Actor _actor;
        /// <summary>
        /// The person that made the update.
        /// </summary>
        public Actor Actor
        {
            get
            {
                return _actor;
            }
            set
            {
                if (_actor != value)
                {
                    _actor = value;
                    RaisePropertyChanged("Actor");
                }
            }
        }

        /// <summary>
        /// The type of update.
        /// </summary>
        public FriendUpdateType UpdateType { get; set; }

        /// <summary>
        /// Type of action.
        /// </summary>
        public string ActionType { get; set; }

        public string ActionRating { get; set; }

        public string ActionShelf { get; set; }

        public string Body { get; set; }

        public BookUpdate Book { get; set; }

        public string UpdateText
        {
            get
            {
                switch (UpdateType)
                {
                    case FriendUpdateType.Review:
                        switch (ActionType)
                        {
                            case "rating":
                                return String.Format("{0} gave {1} stars to {2} by {3}", Actor.Name, ActionRating, Book.Title, "a");
                            case "added":
                                return String.Format("{0} gave {1} stars to {2} by {3}", Actor.Name, ActionRating, Book.Title, "a");
                            default:
                                break;
                        }
                        break;

                    case FriendUpdateType.Comment:
                        break;

                    case FriendUpdateType.UserStatus:
                        break;

                    default:
                        break;
                }

                return ActionText;
            }
        }
    }
}
