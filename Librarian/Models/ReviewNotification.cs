namespace Librarian
{
    public class ReviewNotification : NotificationResource
    {
        public int BookId { get; set; }

        public int Rating { get; set; }
    }
}
