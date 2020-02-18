using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;
using System.Xml.Serialization;
using System.Xml;

namespace Librarian
{
    public class Book : ModelItemBase<IdLoadContext>
    {
        public static Book CreateFromXml(XElement element)
        {
            var book = new Book((string)element.Element("id"));

            book.Title = (string)element.Element("title");
            book.Isbn = (string)element.Element("isbn");
            book.Isbn13 = (string)element.Element("isbn13");
            book.Description = (string)element.Element("description");
            book.CoverUrl = new Uri((string)element.Element("image_url"));
            book.SmallCoverUrl = new Uri((string)element.Element("small_image_url"));
            book.PublicationYear = (string)element.Element("publication_year");
            book.PublicationMonth = (string)element.Element("publication_month");
            book.PublicationDay = (string)element.Element("publication_day");
            book.Publisher = (string)element.Element("publisher");
            book.AverageRating = (string)element.Element("average_rating");
            book.NumberOfPages = (string)element.Element("num_pages");
            book.RatingsCount = (string)element.Element("ratings_count");
            book.Link = (string)element.Element("link");
            book.Authors = (from xAuthor in element.Element("authors").Elements("author")
                            select Author.CreateFromXml(xAuthor))
                            .ToObservable<Author>();

            if (element.Element("book_links") != null)
            {
                book.BookLinks = (from xBookLink in element.Element("book_links").Elements("book_link")
                                  select BookLink.CreateFromXml(xBookLink))
                                  .ToObservable<BookLink>();
            }

            if (element.Element("similar_books") != null)
            {
                book.SimilarBooks = (from xSimilarBook in element.Element("similar_books").Elements("book")
                                     select Book.CreateFromXml(xSimilarBook))
                                     .ToObservable<Book>();
            }

            return book;
        }

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

        private ObservableCollection<Book> _similarBooks = new ObservableCollection<Book>();
        public ObservableCollection<Book> SimilarBooks
        {
            get
            {
                return _similarBooks;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException("value");

                if (_similarBooks != null)
                {
                    _similarBooks.Clear();

                    foreach (var book in value)
                    {
                        _similarBooks.Add(book);
                    }

                }
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

                Book book = (from bookElement in doc.Element("GoodreadsResponse").Elements("book")
                             select Book.CreateFromXml(bookElement))
                             .SingleOrDefault();
                
                book.LoadContext = loadContext;
                
                return book;
            }
        }

        #endregion
    }
}
