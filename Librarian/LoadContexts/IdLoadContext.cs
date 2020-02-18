using AgFx;

namespace Librarian
{
    /// <summary>
    /// A generic LoadContext for classes that only need a single Id property.
    /// </summary>
    public class IdLoadContext : LoadContext
    {
        /// <summary>
        /// Returns the Id associated with the LoadContext.
        /// </summary>
        public string Id
        {
            get { return (string)Identity; }
        }

        public IdLoadContext(string Id) : base(Id) { }
    }
}
