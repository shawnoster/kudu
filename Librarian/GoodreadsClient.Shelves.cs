using System;
using AgFx;
using RestSharp;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        public void AddBookToShelf(string bookId, string shelfName)
        {
            RestRequest request = new RestRequest("shelf/add_to_shelf.xml", Method.POST);
            request.AddParameter("name", shelfName);
            request.AddParameter("book_id", bookId);

            _client.ExecuteAsync(request,
                resp =>
                {
                    if (resp.StatusCode != System.Net.HttpStatusCode.Created)
                        throw new RestRequestException("Failed to add book to shelf", resp.StatusCode, resp.Content);
                });
        }

        public ShelfResult GetShelfResult(string userId)
        {
            return DataManager.Current.Load<ShelfResult>(new IdLoadContext(userId));
        }

        public Shelf GetShelf(string userId, string shelf)
        {
            return DataManager.Current.Load<Shelf>(new ShelfLoadContext(userId, shelf));
        }

        public void AddShelf(string shelfName)
        {
            this.AddShelf(shelfName, null);
        }

        public void AddShelf(string shelfName, Action<IRestResponse> callback)
        {
            var request = new RestRequest("user_shelves.xml", Method.POST);
            request.AddParameter("user_shelf[name]", shelfName);
            _client.ExecuteAsync(request, callback);
        }

        public void DeleteShelf(string shelfId, Action<IRestResponse> callback)
        {
            EnsureIsAuthenticated();

            RestRequest request = new RestRequest()
            {
                Resource = String.Format("user_shelves/{0}.xml", shelfId),
                Method = Method.DELETE
            };

            _client.ExecuteAsync(request, callback);
        }

        public void EditShelf(string shelfId, bool featured, bool exclusive, string newName, bool sortable, Action<IRestResponse> callback)
        {
            EnsureIsAuthenticated();

            RestRequest request = new RestRequest()
            {
                Resource = String.Format("user_shelves/{0}.xml", shelfId),
                Method = Method.PUT
            };

            request.AddParameter("user_shelf[featured]", featured.ToString());
            request.AddParameter("user_shelf[exclusive_flag]", exclusive.ToString());
            request.AddParameter("user_shelf[name]", newName);
            request.AddParameter("user_shelf[sortable_flag]", sortable.ToString());

            _client.ExecuteAsync(request, callback);
        }
    }
}
