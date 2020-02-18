using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;

namespace Librarian
{
    [CachePolicy(CachePolicy.CacheThenRefresh)]
    public class UserReview : ModelItemBase<UserReviewLoadContext>
    {
        public string BookId
        {
            get
            {
                return this.LoadContext.BookId;
            }
        }

        public string UserId
        {
            get
            {
                return this.LoadContext.UserId;
            }
        }

        private Book _book;
        public Book Book
        {
            get
            {
                return _book;
            }
            set
            {
                if (_book != value)
                {
                    _book = value;
                    RaisePropertyChanged("Book");
                }
            }
        }

        private string _rating;
        public string Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    RaisePropertyChanged("Rating");
                }
            }
        }

        private int _votes;
        public int Votes
        {
            get
            {
                return _votes;
            }
            set
            {
                if (_votes != value)
                {
                    _votes = value;
                    RaisePropertyChanged("Votes");
                }
            }
        }

        private DateTime? _startedAt;
        public DateTime? StartedAt
        {
            get
            {

                return _startedAt;
            }
            set
            {

                if (_startedAt != value)
                {

                    _startedAt = value;
                    RaisePropertyChanged("StartedAt");
                }
            }
        }

        private DateTime? _readAt;
        public DateTime? ReadAt
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
                    RaisePropertyChanged("ReadAt");
                }
            }
        }

        private DateTime? _dateAdded;
        public DateTime? DateAdded
        {
            get
            {
                return _dateAdded;
            }
            set
            {
                if (_dateAdded != value)
                {
                    _dateAdded = value;
                    RaisePropertyChanged("DateAdded");
                }
            }
        }

        private DateTime? _dateUpdated;
        public DateTime? DateUpdated
        {
            get
            {
                return _dateUpdated;
            }
            set
            {
                if (_dateUpdated != value)
                {
                    _dateUpdated = value;
                    RaisePropertyChanged("DateUpdated");
                }
            }
        }

        private string _readCount;
        public string ReadCount
        {
            get
            {
                return _readCount;
            }
            set
            {
                if (_readCount != value)
                {
                    _readCount = value;
                    RaisePropertyChanged("ReadCount");
                }
            }
        }

        private string _commentsCount;
        public string CommentsCount
        {
            get
            {
                return _commentsCount;
            }
            set
            {
                if (_commentsCount != value)
                {
                    _commentsCount = value;
                    RaisePropertyChanged("CommentsCount");
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
                if (Shelves.Count > 0)
                {
                    return string.Join(",", Shelves);
                }
                else
                {
                    return "add to shelf";
                }
            }
        }

        public int PercentageComplete
        {
            get
            {
                var status = _statuses.FirstOrDefault();
                if (status != null)
                {
                    return Convert.ToInt32(status.Percent);
                }
                else
                {
                    return 0;
                }
            }
        }

        private ObservableCollection<UserStatus> _statuses = new ObservableCollection<UserStatus>();
        public ObservableCollection<UserStatus> Statuses
        {
            get
            {
                return _statuses;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                _statuses.Clear();
                foreach (var s in value)
                {
                    _statuses.Add(s);
                }
            }
        }

        private ObservableCollection<string> _shelves = new ObservableCollection<string>();
        public ObservableCollection<string> Shelves
        {
            get
            {
                return _shelves;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                _shelves.Clear();
                foreach (var s in value)
                {
                    _shelves.Add(s);
                }

                RaisePropertyChanged("ShelfNames");
            }
        }

        public UserReview() { }

        public UserReview(string userId, string bookId) : base(new UserReviewLoadContext(userId, bookId)) { }

        #region DataLoader

        public class UserReviewLoader : DataLoaderBase<UserReviewLoadContext>
        {
            public override LoadRequest GetLoadRequest(UserReviewLoadContext loadContext, Type objectType)
            {
                var request = new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("review/show_by_user_and_book.xml"));

                request.AddParameter("key", GoodreadsClient.Current.ConsumerKey);
                request.AddParameter("user_id", loadContext.UserId);
                request.AddParameter("book_id", loadContext.BookId);

                return request;
            }

            public override object Deserialize(UserReviewLoadContext loadContext, Type objectType, System.IO.Stream stream)
            {
                XDocument doc = XDocument.Load(stream);

                var review = (from r in doc.Descendants("review")
                              select new UserReview
                              {
                                  LoadContext = loadContext,
                                  Rating = (string)r.Element("rating"),
                                  Votes = (int?)r.Element("votes") ?? 0,
                                  StartedAt = Parse.GoodreadsDateTime((string)r.Element("started_at")),
                                  ReadAt = Parse.GoodreadsDateTime((string)r.Element("read_at")),
                                  DateAdded = Parse.GoodreadsDateTime((string)r.Element("date_added")),
                                  DateUpdated = Parse.GoodreadsDateTime((string)r.Element("date_updated")),
                                  ReadCount = (string)r.Element("read_count"),
                                  CommentsCount = (string)r.Element("comments_count"),
                                  Book = (from b in r.Elements("book")
                                          select new Book((string)b.Element("id"))
                                          {
                                              Title = "MONKEY",
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
                                  Statuses = (from s in r.Descendants("user_status")
                                              select new UserStatus
                                              {
                                                  Chapter = (string)s.Element("chapter"),
                                                  CommentsCount = (string)s.Element("comments_count"),
                                                  CreatedAt = (string)s.Element("created_at"),
                                                  Id = (string)s.Element("id"),
                                                  LastCommentAt = (string)s.Element("last_comment_at"),
                                                  Page = (string)s.Element("page"),
                                                  Percent = (string)s.Element("percent"),
                                                  RatingsCount = (string)s.Element("ratings_count"),
                                                  UpdatedAt = (string)s.Element("updated_at"),
                                                  WorkId = (string)s.Element("work_id")
                                              }).ToObservable<UserStatus>(),
                                  Shelves = (from s in r.Descendants("shelf") 
                                             select (string)s.Attribute("name")).ToObservable<string>()

                              }).First(); 

                return review;
            }
        }

        #endregion        
    }
}
