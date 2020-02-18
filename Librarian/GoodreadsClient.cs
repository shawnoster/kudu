using System;
using System.Collections.Generic;
using System.Text;
using AgFx;
using RestSharp;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        private const string _baseUrl = "http://www.goodreads.com";
        private readonly RestClient _client;

        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }

        private static GoodreadsClient _current;
        public static GoodreadsClient Current
        {
            get
            {
                if (_current == null)
                {
                    throw new InvalidOperationException("Must call Initialize first.");
                }
                return _current;
            }
        }

        public static void Initialize(string consumerKey, string consumerSecret)
        {
            if (_current != null)
            {
                throw new ArgumentException("GoodreadsClient already initialized.");
            }

            if (_current == null)
            {
                _current = new GoodreadsClient(consumerKey, consumerSecret);
            }
        }

        public GoodreadsClient(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;

            _client = new RestClient
            {
                BaseUrl = _baseUrl,
                UserAgent = "Librarian"
            };
        }

        #region End-point building methods

        public Uri BuildUri(string resource, Dictionary<string, string> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            // add in the developer key, it won't hurt methods that don't require it and prevents
            // having to scatter it across all calls that do.
            parameters.Add("key", ConsumerKey);

            StringBuilder apiParameters = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                apiParameters.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}={1}&", pair.Key, Uri.EscapeDataString(pair.Value));
            }

            UriBuilder uri = new UriBuilder(_client.BaseUrl + resource);
            uri.Query = apiParameters.ToString();

            return uri.Uri;
        }

        public string BuildResource(string resource)
        {
            return UrlCombine(_client.BaseUrl, resource);
        }

        #endregion

        public void Search(string query, Action<IRestResponse> callback)
        {
            var request = new RestRequest("search/index.xml");

            request.AddParameter("key", ConsumerKey);
            request.AddParameter("q", query);

            _client.ExecuteAsync(request, callback);
        }

        public NotificationResult GetNotifications()
        {
            EnsureIsAuthenticated();
            return DataManager.Current.Load<NotificationResult>(new IdLoadContext(AuthorizedUser.Id));
        }

        # region Books

        public Book GetBookDetails(string bookId)
        {
            return DataManager.Current.Load<Book>(bookId);
        }

        public UserReview GetUsersReview(string bookId)
        {
            return GetUsersReview(AuthorizedUser.Id, bookId);
        }

        public UserReview GetUsersReview(string userId, string bookId)
        {
            return DataManager.Current.Load<UserReview>(new UserReviewLoadContext(userId, bookId));
        }

        public UserReview GetUsersReview(string userId, string bookId, Action<UserReview> completed, Action<Exception> error)
        {
            return DataManager.Current.Load<UserReview>(new UserReviewLoadContext(userId, bookId), completed, error);
        }

        #endregion

        #region Author

        public void GetBooksByAuthor(string authorId, int page, Action<IRestResponse> callback)
        {
            RestRequest request = new RestRequest { Resource = "author/list.xml" };

            request.AddParameter("key", ConsumerKey);
            request.AddParameter("id", authorId);
            request.AddParameter("page", page.ToString());

            _client.ExecuteAsync(request, callback);
        }

        #endregion

        public void GetOwnedBooksAsync(Action<IRestResponse> callback)
        {
            EnsureIsAuthenticated();

            RestRequest request = new RestRequest()
            {
                Resource = "owned_books/user?format=xml"
            };

            _client.ExecuteAsync(request, callback);
        }

        #region User Status

        public void UpdateUserStatus(string body, Action<IRestResponse> callback)
        {
            var request = new RestRequest("user_status.xml", Method.POST);
            request.AddParameter("user_status[body]", body);

            _client.ExecuteAsync(request, callback);
        }

        public void UpdateBookPage(string bookId, string page, string comment, Action<IRestResponse> callback)
        {
            var request = new RestRequest("user_status.xml", Method.POST);
            request.AddParameter("user_status[book_id]", bookId);

            if (!String.IsNullOrEmpty(page)) request.AddParameter("user_status[page]", page);
            if (!String.IsNullOrEmpty(comment)) request.AddParameter("user_status[body]", comment);

            _client.ExecuteAsync(request, callback);
        }

        public void UpdateBookPercent(string bookId, int percent, Action<IRestResponse> callback)
        {
            var request = new RestRequest("user_status.xml", Method.POST);
            request.AddParameter("user_status[book_id]", bookId);
            request.AddParameter("user_status[percent]", percent.ToString());

            _client.ExecuteAsync(request, callback);
        }

        #endregion        

        private static string UrlCombine(string baseUrl, string resource)
        {
            if (baseUrl.Length == 0)
            {
                return baseUrl;
            }

            if (resource.Length == 0)
            {
                return resource;
            }

            baseUrl = baseUrl.TrimEnd(new char[] { '/', '\\' });
            resource = resource.TrimStart(new char[] { '/', '\\' });

            return String.Format("{0}/{1}", baseUrl, resource);
        }
    }
}