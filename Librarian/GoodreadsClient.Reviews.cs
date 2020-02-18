using System;
using RestSharp;
using AgFx;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        /// <summary>
        /// Add a book review to the currently authenticated user.
        /// </summary>
        /// <param name="bookId">Goodreads book id.</param>
        /// <param name="shelf">read|currently-reading|to-read|user shelf name, default is read</param>
        /// <param name="review">Text of the review</param>
        /// <param name="readAt">Date finished</param>
        /// <param name="rating">Rating (0-5), default is 0 (no rating)</param>
        /// <param name="callback">Callback to call when add completes</param>
        public void AddReview(string bookId, string shelf, string review, DateTime? readAt, int rating, Action<IRestResponse> callback)
        {
            EnsureIsAuthenticated();

            RestRequest request = new RestRequest("review.xml", Method.POST);            

            request.AddParameter("book_id", bookId);
            if (!string.IsNullOrEmpty(shelf)) request.AddParameter("shelf", shelf);
            if (!string.IsNullOrEmpty(review)) request.AddParameter("review[review]", review);
            if (readAt.HasValue) request.AddParameter("review[read_at]", String.Format("{0:yyyy-MM-dd}", readAt));
            request.AddParameter("review[rating]", rating);

            _client.ExecuteAsync(request, callback);
        }
    }
}
