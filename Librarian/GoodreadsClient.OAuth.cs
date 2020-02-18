using System;
using System.Net;
using System.Xml.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Librarian.Common;

namespace Librarian
{
    public partial class GoodreadsClient
    {
        public static Uri GetAuthorizationUri(OAuthRequestToken oauth)
        {
            if (oauth == null)
                throw new ArgumentNullException("oauth");

            return new Uri("http://www.goodreads.com/oauth/authorize?mobile=1&oauth_token=" + oauth.Token);
        }

        public bool IsAuthenticated { get; private set; }        

        public void GetRequestToken(string callbackUrl, Action<OAuthRequestToken> action)
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = OAuth1Authenticator.ForRequestToken(ConsumerKey, ConsumerSecret, callbackUrl);
            var request = new RestRequest("oauth/request_token");

            client.ExecuteAsync(request,
                resp =>
                {
                    // Handle any non-OK status codes
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        XDocument doc = XDocument.Parse(resp.Content);
                        var errorMessage = (string)doc.Element("errors").Element("error");
                        throw new RestRequestException(errorMessage, resp.StatusCode);
                    }

                    var parameters = Parse.QueryString(resp.Content);
                    var requestToken = new OAuthRequestToken
                    {                        
                        Token = parameters["oauth_token"],
                        TokenSecret = parameters["oauth_token_secret"]
                    };

                    if (String.IsNullOrEmpty(requestToken.Token) || String.IsNullOrEmpty(requestToken.TokenSecret))
                    {
                        throw new RestRequestException("Error while attempting to request a token.", resp.StatusCode, resp.Content);
                    }

                    action(requestToken);
                }); 
        }

        public void GetAccessToken(OAuthRequestToken requestToken, string verifier, Action<OAuthAccessToken> action)
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, requestToken.Token, requestToken.TokenSecret, verifier);
            var request = new RestRequest("oauth/access_token");

            client.ExecuteAsync(request,
                (resp) =>
                {
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        XDocument doc = XDocument.Parse(resp.Content);
                        var errorMessage = (string)doc.Element("errors").Element("error");
                        throw new RestRequestException(errorMessage, resp.StatusCode);
                    }

                    var parameters = Parse.QueryString(resp.Content);
                    var accessToken = new OAuthAccessToken
                    {
                        Token = parameters["oauth_token"],
                        TokenSecret = parameters["oauth_token_secret"]
                    };

                    if (String.IsNullOrEmpty(accessToken.Token) || String.IsNullOrEmpty(accessToken.TokenSecret))
                    {
                        throw new RestRequestException("Error retrieving access token.", resp.StatusCode, resp.Content);
                    }

                    action(accessToken);
                });
        }
    }
}