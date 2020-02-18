using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;
using System.IO;

namespace Librarian
{
    public class FriendUpdateResult : ModelItemBase<IdLoadContext>
    {
        private ObservableCollection<FriendUpdate> _updates = new ObservableCollection<FriendUpdate>();
        public ObservableCollection<FriendUpdate> Updates
        {
            get
            {
                return _updates;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                if (_updates != null)
                {
                    _updates.Clear();

                    foreach (var s in value)
                    {
                        _updates.Add(s);
                    }

                }                
            }
        }

        public class FriendUpdateResultDataLoader : DataLoaderBase<IdLoadContext>
        {
            public override LoadRequest GetLoadRequest(IdLoadContext loadContext, Type objectType)
            {
                var request = new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("updates/friends.xml"),
                    GoodreadsClient.Current.ConsumerKey,
                    GoodreadsClient.Current.ConsumerSecret,
                    GoodreadsClient.Current.AccessToken,
                    GoodreadsClient.Current.AccessTokenSecret);

                return request;
            }

            public override object Deserialize(IdLoadContext loadContext, Type objectType, System.IO.Stream stream)
            {
                FriendUpdateResult result = new FriendUpdateResult
                {
                    LoadContext = loadContext
                };

                XDocument doc = XDocument.Load(stream);

                result.Updates = (from u in doc.Descendants("update")
                                  let type = ToUpdateType((string)u.Attribute("type"))
                                  select new FriendUpdate
                                  {
                                      UpdateType = type,
                                      ActionText = (string)u.Element("action_text"),
                                      Link = (string)u.Element("link"),
                                      ImageUrl = (string)u.Element("image_url"),
                                      UpdatedAt = Convert.ToDateTime((string)u.Element("updated_at")),
                                      Actor = ParseActor(u),
                                      Body = (string)u.Element("body"),
                                      ActionType = (type == FriendUpdateType.Review) ? (string)u.Element("action").Attribute("type") : null,
                                      ActionRating = (type == FriendUpdateType.Review) ? (string)u.Element("action").Element("rating") : null,
                                      ActionShelf = (type == FriendUpdateType.Review) ? (string)u.Element("action").Element("shelf") : null,
                                      Book = (type == FriendUpdateType.Review) ? ParseBookUpdate(u.Element("object").Element("book")) : null
                                  }).ToObservable<FriendUpdate>();

                return result;
            }

            private static FriendUpdateType ToUpdateType(string s)
            {
                switch (s)
                {
                    case "review":
                        return FriendUpdateType.Review;
                    case "comment":
                        return FriendUpdateType.Comment;
                    case "userstatus":
                        return FriendUpdateType.UserStatus;
                    case "userlistvote":
                        return FriendUpdateType.UserListVote;
                    case "userquote":
                        return FriendUpdateType.UserQuote;
                    case "friend":
                        return FriendUpdateType.Friend;
                    case "recommendation":
                        return FriendUpdateType.Recommentation;
                    default:
                        throw new ArgumentOutOfRangeException(s, "Unknown UpdateType");
                }
            }

            private static Actor ParseActor(XElement element)
            {
                return (from a in element.Elements("actor")
                        select new Actor
                        {
                            Id = (string)a.Element("id"),
                            Name = (string)a.Element("name"),
                            ImageUrl = new Uri((string)a.Element("image_url"), UriKind.Absolute),
                            Link = new Uri((string)a.Element("link"), UriKind.Absolute)
                        }).Single();
            }

            private static BookUpdate ParseBookUpdate(XElement element)
            {
                return new BookUpdate
                {
                    Id = (string)element.Element("id"),
                    Title = (string)element.Element("title"),
                    Link = (string)element.Element("link")
                };
            }
        }
    }
}
