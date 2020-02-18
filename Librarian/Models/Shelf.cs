using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;
using RestSharp.Deserializers;
using RestSharp;

namespace Librarian
{
    [CachePolicy(CachePolicy.CacheThenRefresh, 60)]
    public class Shelf : ModelItemBase<ShelfLoadContext>
    {
        public string UserId
        {
            get
            {
                return this.LoadContext.UserId;
            }
        }

        /// <summary>
        /// The name of the shelf, there are several pre-defined shelf names and the user can create
        /// as many shelves as they'd like.
        /// </summary>
        public string Name
        {
            get
            {
                return this.LoadContext.ShelfName;
            }
        }

        /// <summary>
        /// The number of books currently in the shelf.
        /// </summary>
        private int _bookCount;
        public int BookCount
        {
            get
            {
                return _bookCount;
            }
            set
            {
                if (_bookCount != value)
                {
                    _bookCount = value;
                    RaisePropertyChanged("BookCount");
                    RaisePropertyChanged("NameAndCount");
                }
            }
        }

        /// <summary>
        /// A description of the shelf, it can (and often will be), empty.
        /// </summary>
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Whether or not a shelf is exclusive.
        /// </summary>
        private bool _exclusive;
        public bool Exclusive
        {
            get
            {
                return _exclusive;
            }
            set
            {
                if (_exclusive != value)
                {
                    _exclusive = value;
                    RaisePropertyChanged("Exclusive");
                }
            }
        }

        /// <summary>
        /// Returns a string with the shelf name and book count in one line.
        /// </summary>
        public string NameAndCount
        {
            get
            {
                return String.Format("{0} ({1})", Name, BookCount);
            }
        }

        /// <summary>
        /// A collection of the books in a shelf.
        /// </summary>
        private ObservableCollection<UserReview> _reviews = new ObservableCollection<UserReview>();
        public ObservableCollection<UserReview> Reviews
        {
            get
            {
                return _reviews;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                if (_reviews != null)
                {
                    _reviews.Clear();

                    foreach (var s in value)
                    {
                        _reviews.Add(s);
                    }

                }
            }
        }

        public Shelf() { }

        public Shelf(string userId, string shelf) : base(new ShelfLoadContext(userId, shelf)) { }

        #region DataLoader
        public class ShelfLoader : DataLoaderBase<ShelfLoadContext>
        {
            public override LoadRequest GetLoadRequest(ShelfLoadContext loadContext, Type objectType)
            {
                var request = new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("review/list/{id}.xml"),
                    GoodreadsClient.Current.ConsumerKey,
                    GoodreadsClient.Current.ConsumerSecret,
                    GoodreadsClient.Current.AccessToken,
                    GoodreadsClient.Current.AccessTokenSecret);
                request.AddUrlSegment("id", loadContext.UserId);

                request.AddParameter("key", GoodreadsClient.Current.ConsumerKey);
                request.AddParameter("shelf", loadContext.ShelfName);
                request.AddParameter("v", "2");
                request.AddParameter("page", loadContext.Page.ToString());

                return request;
            }

            public override object Deserialize(ShelfLoadContext loadContext, Type objectType, Stream stream)
            {
                if (loadContext == null)
                    throw new ArgumentNullException("loadContext");

             
                XDocument doc = XDocument.Load(stream);

                var reviews = (from r in doc.Descendants("reviews")
                               select r).FirstOrDefault();

                var shelf = new Shelf
                {
                    LoadContext = loadContext,
                    BookCount = (int)reviews.Attribute("total")
                };

                shelf.Reviews = (from review in reviews.Descendants("review")
                                 select new UserReview(loadContext.UserId, (string)review.Element("book").Element("id"))
                                 {
                                     Book = (from b in review.Elements("book")
                                             select new Book((string)b.Element("id"))
                                             {
                                                 Title = (string)b.Element("title"),
                                                 CoverUrl = new Uri((string)b.Element("image_url")),
                                                 NumberOfPages = (string)b.Element("num_pages"),
                                                 AverageRating = (string)b.Element("average_rating"),
                                                 Description = (string)b.Element("description").Value,
                                                 Authors = (from a in b.Descendants("author")
                                                            select new Author
                                                            {
                                                                Name = (string)a.Element("name")
                                                            }).ToObservable<Author>()
                                             }).SingleOrDefault(),
                                     Shelves = (from s in review.Descendants("shelf")
                                                select (string)s.Attribute("name")).ToObservable<string>(),
                                     Votes = (int?)review.Element("votes") ?? 0,
                                     ReadAt = Parse.GoodreadsDateTime((string)review.Element("read_at")),
                                     StartedAt = Parse.GoodreadsDateTime((string)review.Element("started_at")),
                                     DateAdded = Parse.GoodreadsDateTime((string)review.Element("date_added")),
                                     DateUpdated = Parse.GoodreadsDateTime((string)review.Element("date_updated"))
                                 }).ToObservable<UserReview>();

                return shelf;
            }
        }
        #endregion        
    }
}
