using System;
using System.Xml.Linq;
using Librarian.Common;

namespace Librarian
{
    public class Author
    {
        public static Author CreateFromXml(XElement element)
        {
            return new Author
            {                
                Id = (string)element.Element("id"),
                Name = (string)element.Element("name"),
                ImageUrl = Parse.GoodreadsUri((string)element.Element("image_url")),
                SmallImageUrl = Parse.GoodreadsUri((string)element.Element("small_image_url")),
                Link = Parse.GoodreadsUri((string)element.Element("link")),
                AverageRating = (string)element.Element("average_rating"),
                RatingsCount = (string)element.Element("ratings_count"),
                TextReviewsCount = (string)element.Element("text_reviews_count")
            };
        }

        public string Id { get; set; }
        
        public string Name { get; set; }                

        public Uri ImageUrl { get; set; }

        public Uri SmallImageUrl { get; set; }

        public Uri Link { get; set; }

        public string AverageRating { get; set; }

        public string RatingsCount { get; set; }

        public string TextReviewsCount { get; set; }
    }
}
