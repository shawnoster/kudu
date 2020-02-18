using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;

namespace Librarian
{
    public class Book : ModelItemBase<IdLoadContext>
    {
        #region XML Parsing
        
        public static Book Parse(XElement xBook)
        {
            var book = new Book((string)xBook.Element("id"));

            book.Title = (string)xBook.Element("title");
            book.Isbn = (string)xBook.Element("isbn");
            book.Isbn13 = (string)xBook.Element("isbn13");
            book.Description = (string)xBook.Element("description");
            book.CoverUrl = new Uri((string)xBook.Element("image_url"));
            book.SmallCoverUrl = new Uri((string)xBook.Element("small_image_url"));
            book.PublicationYear = (string)xBook.Element("publication_year");
            book.PublicationMonth = (string)xBook.Element("publication_month");
            book.PublicationDay = (string)xBook.Element("publication_day");
            book.Publisher = (string)xBook.Element("publisher");
            book.AverageRating = (string)xBook.Element("average_rating");
            book.NumberOfPages = (string)xBook.Element("num_pages");
            book.RatingsCount = (string)xBook.Element("ratings_count");
            book.Link = (string)xBook.Element("link");
            book.Authors = (from xAuthor in xBook.Descendants("author")
                            select new Author
                            {
                                Id = (string)xAuthor.Element("id"),
                                Name = (string)xAuthor.Element("name"),
                                Link = (string)xAuthor.Element("link")
                            }).ToObservable<Author>();
            book.BookLinks = (from xBookLink in xBook.Descendants("book_link")
                              select new BookLink
                              {
                                  Id = (string)xBookLink.Element("id"),
                                  Name = (string)xBookLink.Element("name"),
                                  Link = (string)xBookLink.Element("link")
                              }).ToObservable<BookLink>();

            return book;
        }

        #endregion

        public string BookId
        {
            get { return LoadContext.Id; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        private Uri _coverUrl;
        public Uri CoverUrl
        {
            get { return _coverUrl; }
            set
            {
                if (_coverUrl != value)
                {
                    _coverUrl = value;
                    RaisePropertyChanged("CoverUrl");
                }
            }
        }

        private string _isbn;
        public string Isbn
        {
            get
            {
                return _isbn;
            }
            set
            {
                if (_isbn != value)
                {
                    _isbn = value;
                    RaisePropertyChanged("Isbn");
                }
            }
        }

        private string _isbn13;
        public string Isbn13
        {
            get
            {
                return _isbn13;
            }
            set
            {
                if (_isbn13 != value)
                {
                    _isbn13 = value;
                    RaisePropertyChanged("Isbn13");
                }
            }
        }

        private Uri _smallCoverUrl;
        public Uri SmallCoverUrl
        {
            get
            {
                return _smallCoverUrl;
            }
            set
            {
                if (_smallCoverUrl != value)
                {
                    _smallCoverUrl = value;
                    RaisePropertyChanged("SmallCoverUrl");
                }
            }
        }

        private string _publicationYear;
        public string PublicationYear
        {
            get
            {
                return _publicationYear;
            }
            set
            {
                if (_publicationYear != value)
                {
                    _publicationYear = value;
                    RaisePropertyChanged("PublicationYear");
                }
            }
        }

        private string _publicationMonth;
        public string PublicationMonth
        {
            get
            {
                return _publicationMonth;
            }
            set
            {
                if (_publicationMonth != value)
                {
                    _publicationMonth = value;
                    RaisePropertyChanged("PublicationMonth");
                }
            }
        }

        private string _publicationDay;
        public string PublicationDay
        {
            get
            {
                return _publicationDay;
            }
            set
            {
                if (_publicationDay != value)
                {
                    _publicationDay = value;
                    RaisePropertyChanged("PublicationDay");
                }
            }
        }

        private string _publisher;
        public string Publisher
        {
            get
            {
                return _publisher;
            }
            set
            {
                if (_publisher != value)
                {
                    _publisher = value;
                    RaisePropertyChanged("Publisher");
                }
            }
        }

        private string _averageRating;
        public string AverageRating
        {
            get
            {
                return _averageRating;
            }
            set
            {
                if (_averageRating != value)
                {
                    _averageRating = value;
                    RaisePropertyChanged("AverageRating");
                }
            }
        }

        private string _ratingsCount;
        public string RatingsCount
        {
            get
            {
                return _ratingsCount;
            }
            set
            {
                if (_ratingsCount != value)
                {
                    _ratingsCount = value;
                    RaisePropertyChanged("RatingsCount");
                }
            }
        }

        private string _numberOfPages;
        public string NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                if (_numberOfPages != value)
                {
                    _numberOfPages = value;
                    RaisePropertyChanged("NumberOfPages");
                }
            }
        }

        private string _link;
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                if (_link != value)
                {
                    _link = value;
                    RaisePropertyChanged("Link");
                }
            }
        }

        private ObservableCollection<Author> _authors = new ObservableCollection<Author>();
        public ObservableCollection<Author> Authors
        {
            get
            {
                return _authors;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                _authors.Clear();
                foreach (var author in value)
                {
                    _authors.Add(author);
                }

                RaisePropertyChanged("By");
            }
        }

        public string By
        {
            get
            {
                if ((Authors != null) && (Authors.Count > 0))
                {
                    string byLine = "by ";
                    foreach (var a in Authors)
                    {
                        byLine += a.Name + ", ";
                    }
                    return byLine.TrimEnd(new char[] { ',', ' ' });
                }
                else
                {
                    return "";
                }
            }
        }

        private ObservableCollection<BookLink> _bookLinks = new ObservableCollection<BookLink>();
        public ObservableCollection<BookLink> BookLinks
        {
            get
            {
                return _bookLinks;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                if (_bookLinks != null)
                {
                    _bookLinks.Clear();

                    foreach (var link in value)
                    {
                        _bookLinks.Add(link);
                    }

                }                
            }
        }

        public Book() { }

        public Book(string bookId) : base(new IdLoadContext(bookId)) { }

        #region DataLoader
        public class BookLoader : DataLoaderBase<IdLoadContext>
        {
            public override LoadRequest GetLoadRequest(IdLoadContext loadContext, System.Type objectType)
            {
                var request = new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource(String.Format("book/show/{0}", loadContext.Id)));

                request.AddParameter("format", "xml");
                request.AddParameter("key", GoodreadsClient.Current.ConsumerKey);

                return request;
            }

            public override object Deserialize(IdLoadContext loadContext, Type objectType, Stream stream)
            {
                XDocument doc = XDocument.Load(stream);

                //var xBook = (from b in doc.Descendants("book")
                //            select b).SingleOrDefault();

                var bookFragment = doc.Element("GoodreadsResponse").Element("book");

                var book = Book.Parse(bookFragment);
                book.LoadContext = loadContext;

                return book;
            }
        }
        #endregion
    }
}
