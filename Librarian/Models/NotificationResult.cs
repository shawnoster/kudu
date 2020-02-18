using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;

namespace Librarian
{
    public class NotificationResult : ModelItemBase<IdLoadContext>
    {
        private ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return _notifications;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                if (_notifications != null)
                {
                    _notifications.Clear();
                    foreach (var s in value)
                    {
                        _notifications.Add(s);
                    }

                }
            }
        }

        public class NotificationResultDataLoader : DataLoaderBase<IdLoadContext>
        {
            public override LoadRequest GetLoadRequest(IdLoadContext loadContext, Type objectType)
            {
                return new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("notifications.xml"),
                    GoodreadsClient.Current.ConsumerKey,
                    GoodreadsClient.Current.ConsumerSecret,
                    GoodreadsClient.Current.AccessToken,
                    GoodreadsClient.Current.AccessTokenSecret);
            }

            public override object Deserialize(IdLoadContext loadContext, Type objectType, System.IO.Stream stream)
            {
                NotificationResult result = new NotificationResult
                {
                    LoadContext = loadContext
                };

                XDocument doc = XDocument.Load(stream);

                result.Notifications = (from n in doc.Descendants("notification")
                                        select new Notification
                                        {
                                            Actors = ParseActor(n.Element("actors")),
                                            IsNew = (bool)n.Element("new"),
                                            CreatedAt = Convert.ToDateTime((string)n.Element("created_at")),
                                            ResourceType = (string)n.Element("resource_type"),
                                            GroupResourceType = (string)n.Element("group_resource_type"),
                                            BodyHtml = (string)n.Element("body").Element("html"),
                                            BodyText = ((string)n.Element("body").Element("text")).Trim(),
                                            Url = Parse.GoodreadsUri((string)n.Element("url")),
                                            Topic = ParseTopic(n.Element("group_resource").Elements("topic")),
                                            Review = ParseReview(n.Descendants("review"))
                                        }).ToObservable<Notification>();

                return result;
            }

            private static TopicNotification ParseTopic(IEnumerable<XElement> elements)
            {
                return (from t in elements
                        select new TopicNotification
                        {
                            
                            Title = (string)t.Element("title"),
                            CreatedAt = Convert.ToDateTime((string)t.Element("created_at")),
                            LastCommentAt = Convert.ToDateTime((string)t.Element("last_comment_at")),
                            UpdatedAt = Convert.ToDateTime((string)t.Element("updated_at"))
                        }).FirstOrDefault();
            }

            private static ReviewNotification ParseReview(IEnumerable<XElement> elements)
            {
                return (from t in elements
                        select new ReviewNotification
                        {
                            BookId = (int)t.Element("book_id"),
                            Rating = (int)t.Element("rating"),
                            CreatedAt = Convert.ToDateTime((string)t.Element("created_at")),
                            LastCommentAt = Convert.ToDateTime((string)t.Element("last_comment_at")),
                            UpdatedAt = Convert.ToDateTime((string)t.Element("updated_at"))
                        }).FirstOrDefault();
            }

            private static ReadOnlyCollection<Actor> ParseActor(XElement element)
            {
                return (from a in element.Elements("user")
                        select new Actor
                        {
                            Id = (string)a.Element("id"),
                            Name = (string)a.Element("name"),
                            Location = (string)a.Element("location"),
                            Link = new Uri((string)a.Element("link"), UriKind.Absolute),                            
                            ImageUrl = new Uri((string)a.Element("image_url"), UriKind.Absolute),                            
                            SmallImageUrl = new Uri((string)a.Element("small_image_url"), UriKind.Absolute)
                        }).ToList<Actor>().AsReadOnly();
            }
        }
    }
}
