using System;
using AgFx;
using RestSharp;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        public void AddFriend(string friendId, Action<IRestResponse> callback)
        {
            EnsureIsAuthenticated();

            RestRequest request = new RestRequest("friend/add_as_friend.xml", Method.POST);
            request.AddParameter("id", friendId);

            _client.ExecuteAsync(request, callback);
        }

        public FriendUpdateResult GetFriendUpdates()
        {
            EnsureIsAuthenticated();
            return DataManager.Current.Load<FriendUpdateResult>(new IdLoadContext(AuthorizedUser.Id));
        }
    }
}
