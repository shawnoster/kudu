using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using AgFx;
using RestSharp;
using RestSharp.Authenticators;

namespace Librarian
{
    public class RestSharpLoadRequest : LoadRequest
    {
        private RestRequest _request;
        private string _consumerKey;
        private string _consumerSecret;
        private string _accessToken;
        private string _accessTokenSecret;

        /// <summary>
        /// The Uri to request
        /// </summary>
        public string Resource
        {
            get
            {
                return _request.Resource;
            }
            set
            {
                _request.Resource = value;
            }
        }

        /// <summary>
        /// Does the request require authentication. Returns true if all four 
        /// authentication properties are set to non-null, non-empty. 
        /// Otherwise false.
        /// </summary>
        public bool IsProtectedResource { get; private set; }

        /// <summary>
        /// The method to use.
        /// </summary>
        public Method Method
        {
            get
            {
                return _request.Method;
            }
            set
            {
                _request.Method = value;
            }
        }

        /// <summary>
        /// Create a RestSharpLoadRequest
        /// </summary>
        /// <param name="loadContext"></param>
        /// <param name="resource"></param>
        public RestSharpLoadRequest(LoadContext loadContext, string resource)
            : base(loadContext)
        {
            _request = new RestRequest(resource);   
            _request.AddHeader("Accept-Encoding", "gzip");
        }

        /// <summary>
        /// Create a RestSharpLoadRequest
        /// </summary>
        /// <param name="loadContext"></param>
        /// <param name="resource">The URI to request</param>
        /// <param name="consumerKey">The consumer key used to make the protected resource request</param>
        /// <param name="consumerSecret">The consumer secret key used to make the protected resource request</param>
        /// <param name="accessToken">The access token used to make the protected resource request</param>
        /// <param name="accessTokenSecret">The access token secret to make the protected resource request</param>
        public RestSharpLoadRequest(LoadContext loadContext, string resource, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
            : this(loadContext, resource)
        {
            IsProtectedResource = true;

            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
        }

        /// <summary>
        /// Adds a parameter to the current request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParameter(string name, object value)
        {
            _request.AddParameter(name, value);
        }

        /// <summary>
        /// Replaces a named segment in the resource with the specificed value.
        /// </summary>
        /// <param name="name">Name of the key in the resource uri.</param>
        /// <param name="value">Value to replace the named key with.</param>
        public void AddUrlSegment(string name, string value)
        {
            _request.AddUrlSegment(name, value);
        }

        /// <summary>
        /// Override this to take a closer look at the response, for example to look at the status code.
        /// 
        /// Default implementation looks for HttpResponse.StatusCode == 200.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual bool IsGoodResponse(IRestResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Performs the actual HTTP get for this request.
        /// </summary>
        /// <param name="result"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void Execute(Action<LoadRequestResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            PriorityQueue.AddNetworkWorkItem(
                () =>
                {
                    var client = new RestClient
                    {
                        UserAgent = "AgFx",                       
                    };

                    if (IsProtectedResource)
                    {
                        client.Authenticator = OAuth1Authenticator.ForProtectedResource(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret);
                    }                    

                    client.ExecuteAsync(
                        _request,
                        resp =>
                        {
                            if (IsGoodResponse(resp))
                            {
                                byte[] byteArray = Encoding.UTF8.GetBytes(resp.Content);
                                result(new LoadRequestResult(new MemoryStream(byteArray)));
                            }
                            else
                            {
                                result(new LoadRequestResult(new RestRequestException(resp.StatusDescription, resp.StatusCode, resp.Content)));
                            }
                            return;
                        });
                });
        }

    }
}