using System.Xml.Linq;

namespace Librarian
{
    public class BookLink
    {
        public static BookLink CreateFromXml(XElement element)
        {
            return new BookLink 
            {
                Id = (string)element.Element("id"),
                Name = (string)element.Element("name"),
                Link = (string)element.Element("link")
            };
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
    }
}
