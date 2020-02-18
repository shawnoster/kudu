using System;
using RestSharp.Authenticators;
using AgFx;
using System.Net;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }
        public AuthorizedUser AuthorizedUser { get; private set; }

        public void AuthenticateWith(string accessToken, string accessTokenSecret)
        {
            if ((AccessToken != accessToken) && (AccessTokenSecret != accessTokenSecret))
            {
                AccessToken = accessToken;
                AccessTokenSecret = accessTokenSecret;

                _client.Authenticator = OAuth1Authenticator.ForProtectedResource(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            }
        }

        public void AuthenticateUser()
        {
            AuthenticateUser(null, null);
        }

        public void AuthenticateUser(Action<AuthorizedUser> completed, Action<Exception> error)
        {
            EnsureIsAuthenticated();

            AuthorizedUser = DataManager.Current.LoadFromCache<AuthorizedUser>("authenticated_user");

            // LoadFromCache returns an object even if it didn't exist in the
            // cache so check one of its properties for existance vs. the object
            // itself.
            if (!String.IsNullOrEmpty(AuthorizedUser.Id))
            {
                if (completed != null)
                    completed(AuthorizedUser);
                return;
            }

            // Force a network request of the AuthorizedUser because the code
            // above has just confirmed it doesn't exist in the cache.  This
            // funky deconstruction of what a normal Load<>() does is so the
            // calling code can wait on getting a "real" instance vs. an
            // empty one that then pops with live data.
            DataManager.Current.Refresh<AuthorizedUser>(
                "authenticated_user",
                user =>
                {
                    this.AuthorizedUser = user;
                    if (completed != null)
                        completed(AuthorizedUser);
                },
                e =>
                {
                    if (error != null)
                    {
                        error(e);
                    }
                });
        }

        /// <summary>
        /// Deauthorizes the current user.
        /// </summary>
        public void Deauthenticate()
        {            
            DataManager.Current.Clear<AuthorizedUser>("authenticated_user");

            AccessToken = "";
            AccessTokenSecret = "";
            _client.Authenticator = null;
        }

        private void EnsureIsAuthenticated()
        {
            if (_client.Authenticator == null)
            {
                throw new InvalidOperationException("You must be authenticated before making this call.");
            }
        }
    }
}
