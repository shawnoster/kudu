using AgFx;

namespace Librarian
{
    public class ShelfLoadContext : LoadContext
    {
        public string UserId
        {
            get
            {
                return this.Identity.ToString();
            }
        }        

        public string ShelfName { get; private set; }

        public int Page { get; set; }

        public ShelfLoadContext(string userId, string shelfName)
            : base(userId)
        {
            this.ShelfName = shelfName;
            this.Page = 1;
        }

        protected override string GenerateKey()
        {
            return string.Format("{0}/{1}/{2}", UserId, ShelfName, Page);
        }        
    }
}
