using System;

namespace Librarian
{
    public class Actor
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Uri Link { get; set; }

        public Uri ImageUrl { get; set; }

        public Uri SmallImageUrl { get; set; }

        public string Location { get; set; }
    }
}
