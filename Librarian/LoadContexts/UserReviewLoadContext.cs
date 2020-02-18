using AgFx;

namespace Librarian
{
    public class UserReviewLoadContext : LoadContext
    {
        public UserReviewLoadContext(string userId, string bookId)
            : base(bookId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }

        public string BookId
        {
            get
            {
                return Identity.ToString();
            }
        }
    }
}
