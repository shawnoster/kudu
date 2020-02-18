using System.Xml.Serialization;

namespace Librarian
{
    [XmlRoot("GoodreadsResponse")]
    public class GoodreadsResponse
    {
        //[XmlElement("Request")]
        //public GoodreadsResponseRequest Request { get; set; }

        [XmlElement("books")]
        public GoodreadsResponseBooks Books { get; set; }
    }

    public class GoodreadsResponseRequest
    {
        [XmlElement("authentication")]
        public string Authentication { get; set; }

        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("method")]
        public string Method { get; set; }
    }
    
    public class GoodreadsResponseBooks
    {
        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("book", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        //public GoodreadsResponseBooksBook[] book
        //{
        //    get
        //    {
        //        return this.bookField;
        //    }
        //    set
        //    {
        //        this.bookField = value;
        //    }
        //}

        [XmlAttribute("start")]
        public string Start { get; set; }

        [XmlAttribute("end")]
        public string End { get; set; }

        [XmlAttribute("total")]
        public string Total { get; set; }

        [XmlAttribute("numpages")]
        public string NumPages { get; set; }

        [XmlAttribute("currentpage")]
        public string CurrentPage { get; set; }
    }

    /// <remarks/>
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]    
    
    public partial class GoodreadsResponseBooksBook
    {

        private string titleField;

        private string image_urlField;

        private string small_image_urlField;

        private string linkField;

        private string num_pagesField;

        private string formatField;

        private string edition_informationField;

        private string publisherField;

        private string publication_dayField;

        private string publication_yearField;

        private string publication_monthField;

        private string average_ratingField;

        private string ratings_countField;

        private string descriptionField;

        private string publishedField;

        private GoodreadsResponseBooksBookID[] idField;

        private GoodreadsResponseBooksBookIsbn[] isbnField;

        private GoodreadsResponseBooksBookIsbn13[] isbn13Field;

        private GoodreadsResponseBooksBookText_reviews_count[] text_reviews_countField;

        private GoodreadsResponseBooksBookAuthorsAuthor[][] authorsField;

        /// <remarks/>
        
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        
        public string image_url
        {
            get
            {
                return this.image_urlField;
            }
            set
            {
                this.image_urlField = value;
            }
        }

        /// <remarks/>
        
        public string small_image_url
        {
            get
            {
                return this.small_image_urlField;
            }
            set
            {
                this.small_image_urlField = value;
            }
        }

        /// <remarks/>
        
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        
        public string num_pages
        {
            get
            {
                return this.num_pagesField;
            }
            set
            {
                this.num_pagesField = value;
            }
        }

        /// <remarks/>
        
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        
        public string edition_information
        {
            get
            {
                return this.edition_informationField;
            }
            set
            {
                this.edition_informationField = value;
            }
        }

        /// <remarks/>
        
        public string publisher
        {
            get
            {
                return this.publisherField;
            }
            set
            {
                this.publisherField = value;
            }
        }

        /// <remarks/>
        
        public string publication_day
        {
            get
            {
                return this.publication_dayField;
            }
            set
            {
                this.publication_dayField = value;
            }
        }

        /// <remarks/>
        
        public string publication_year
        {
            get
            {
                return this.publication_yearField;
            }
            set
            {
                this.publication_yearField = value;
            }
        }

        /// <remarks/>
        
        public string publication_month
        {
            get
            {
                return this.publication_monthField;
            }
            set
            {
                this.publication_monthField = value;
            }
        }

        /// <remarks/>
        
        public string average_rating
        {
            get
            {
                return this.average_ratingField;
            }
            set
            {
                this.average_ratingField = value;
            }
        }

        /// <remarks/>
        
        public string ratings_count
        {
            get
            {
                return this.ratings_countField;
            }
            set
            {
                this.ratings_countField = value;
            }
        }

        /// <remarks/>
        
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        
        public string published
        {
            get
            {
                return this.publishedField;
            }
            set
            {
                this.publishedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GoodreadsResponseBooksBookID[] id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("isbn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GoodreadsResponseBooksBookIsbn[] isbn
        {
            get
            {
                return this.isbnField;
            }
            set
            {
                this.isbnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("isbn13", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GoodreadsResponseBooksBookIsbn13[] isbn13
        {
            get
            {
                return this.isbn13Field;
            }
            set
            {
                this.isbn13Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("text_reviews_count", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GoodreadsResponseBooksBookText_reviews_count[] text_reviews_count
        {
            get
            {
                return this.text_reviews_countField;
            }
            set
            {
                this.text_reviews_countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("author", typeof(GoodreadsResponseBooksBookAuthorsAuthor), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GoodreadsResponseBooksBookAuthorsAuthor[][] authors
        {
            get
            {
                return this.authorsField;
            }
            set
            {
                this.authorsField = value;
            }
        }
    }

    /// <remarks/>
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    
    
    public partial class GoodreadsResponseBooksBookID
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
        
    [System.Diagnostics.DebuggerStepThroughAttribute()]    
    
    public partial class GoodreadsResponseBooksBookIsbn
    {

        private string nilField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nil
        {
            get
            {
                return this.nilField;
            }
            set
            {
                this.nilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    
    
    public partial class GoodreadsResponseBooksBookIsbn13
    {

        private string nilField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nil
        {
            get
            {
                return this.nilField;
            }
            set
            {
                this.nilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    
    
    public partial class GoodreadsResponseBooksBookText_reviews_count
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    
    
    public partial class GoodreadsResponseBooksBookAuthorsAuthor
    {

        private string idField;

        private string nameField;

        private string image_urlField;

        private string small_image_urlField;

        private string linkField;

        private string average_ratingField;

        private string ratings_countField;

        private string text_reviews_countField;

        /// <remarks/>
        
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        
        public string image_url
        {
            get
            {
                return this.image_urlField;
            }
            set
            {
                this.image_urlField = value;
            }
        }

        /// <remarks/>
        
        public string small_image_url
        {
            get
            {
                return this.small_image_urlField;
            }
            set
            {
                this.small_image_urlField = value;
            }
        }

        /// <remarks/>
        
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        
        public string average_rating
        {
            get
            {
                return this.average_ratingField;
            }
            set
            {
                this.average_ratingField = value;
            }
        }

        /// <remarks/>
        
        public string ratings_count
        {
            get
            {
                return this.ratings_countField;
            }
            set
            {
                this.ratings_countField = value;
            }
        }

        /// <remarks/>
        
        public string text_reviews_count
        {
            get
            {
                return this.text_reviews_countField;
            }
            set
            {
                this.text_reviews_countField = value;
            }
        }
    }
}