using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using AgFx;
using Librarian.Common;

namespace Librarian
{
    public class ShelfResult : ModelItemBase<IdLoadContext>
    {
        public string UserId
        {
            get
            {
                return LoadContext.Id;
            }
        }

        private ObservableCollection<Shelf> _shelves = new ObservableCollection<Shelf>();
        public ObservableCollection<Shelf> Shelves
        {
            get
            {
                return _shelves;
            }
            set
            {
                if (value == null) throw
                    new ArgumentNullException();

                _shelves.Clear();
                foreach (var s in value)
                {
                    _shelves.Add(s);
                }

                RaisePropertyChanged("ExclusiveShelves");
                RaisePropertyChanged("UserShelves");
            }
        }

        public ReadOnlyCollection<Shelf> ExclusiveShelves
        {
            get
            {
                var exclusiveShelves = from s in _shelves
                                       where s.Exclusive
                                       select s;

                return new ReadOnlyCollection<Shelf>(exclusiveShelves.ToList<Shelf>());
            }
        }

        public ReadOnlyCollection<Shelf> UserShelves
        {
            get
            {
                var userShelves = from s in _shelves
                                       where !s.Exclusive
                                       select s;

                return new ReadOnlyCollection<Shelf>(userShelves.ToList<Shelf>());
            }
        }

        public ShelfResult() { }

        public ShelfResult(string userId) : base(new IdLoadContext(userId)) { }

        #region Data Loader

        public class ShelfCollectionDataLoader : DataLoaderBase<IdLoadContext>
        {
            public override LoadRequest GetLoadRequest(IdLoadContext loadContext, Type objectType)
            {
                var request = new RestSharpLoadRequest(
                    loadContext,
                    GoodreadsClient.Current.BuildResource("shelf/list.xml"));

                request.AddParameter("key", GoodreadsClient.Current.ConsumerKey);
                request.AddParameter("user_id", loadContext.Id);

                return request;
            }

            public override object Deserialize(IdLoadContext loadContext, Type objectType, System.IO.Stream stream)
            {
                if (loadContext == null)
                    throw new ArgumentNullException("loadContext");

                ShelfResult result = new ShelfResult(loadContext.Id);

                XDocument doc = XDocument.Load(stream);
                result.Shelves = (from shelf in doc.Descendants("user_shelf")
                                  select new Shelf((string)shelf.Element("id"), (string)shelf.Element("name"))
                                  {
                                      BookCount = (int)shelf.Element("book_count"),
                                      Description = (string)shelf.Element("description"),
                                      Exclusive = (bool)shelf.Element("exclusive_flag")
                                  }).ToObservable<Shelf>();

                return result;
            }
        }

        #endregion
    }
}
