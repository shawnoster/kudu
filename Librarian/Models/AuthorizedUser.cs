using System.Linq;
using System.Xml.Linq;
using AgFx;

namespace Librarian
{
    [CachePolicy(CachePolicy.Forever)]
    public class AuthorizedUser : ModelItemBase
    {
        public AuthorizedUser()
        {
        }

        public AuthorizedUser(string userType)
            : this()
        {
            this.LoadContext = new LoadContext(userType);
        }

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    RaisePropertyChanged("UserName");
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

        public class AuthorizedUserLoader : DefaultDataLoader
        {
            public override LoadRequest GetLoadRequest(LoadContext loadContext, System.Type objectType)
            {
                return new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("api/auth_user"),
                    GoodreadsClient.Current.ConsumerKey,
                    GoodreadsClient.Current.ConsumerSecret,
                    GoodreadsClient.Current.AccessToken,
                    GoodreadsClient.Current.AccessTokenSecret);
            }

            public override object Deserialize(LoadContext loadContext, System.Type objectType, System.IO.Stream stream)
            {
                XDocument doc = XDocument.Load(stream);

                AuthorizedUser user = (from u in doc.Descendants("user")
                                      select new AuthorizedUser
                                      {
                                          LoadContext = loadContext,
                                          Id = (string)u.Attribute("id"),
                                          UserName = (string)u.Element("name"),
                                          Link = (string)u.Element("link")
                                      }).FirstOrDefault();

                return user;
            }
        }
    }
}
