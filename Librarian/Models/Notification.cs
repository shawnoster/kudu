using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Librarian
{
    public class Notification
    {
        public string ResourceType { get; set; }

        public string GroupResourceType { get; set; }

        public DateTime CreatedAt { get; set; }

        public TopicNotification Topic { get; set; }

        public ReviewNotification Review { get; set; }

        public ReadOnlyCollection<Actor> Actors { get; set; }

        public bool IsNew { get; set; }

        public string BodyHtml { get; set; }

        public string ActorNames
        {
            get
            {
                if (Actors.Count == 0)
                    return String.Empty;

                if (Actors.Count == 1)
                    return Actors[0].Name;

                var sb = new StringBuilder();
                for (int i = 0; i < Actors.Count - 1; i++)
                {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(Actors[i].Name);                    
                }

                sb.Append(" and ");
                sb.Append(Actors[Actors.Count - 1].Name);

                return sb.ToString();
            }
        }

        public Uri ImageUrl
        {
            get
            {
                if (Actors.Count == 0)
                    return null;

                return Actors[0].ImageUrl;
            }
        }

        public Uri SmallImageUrl
        {
            get
            {
                if (Actors.Count == 0)
                    return null;

                return Actors[0].SmallImageUrl;
            }
        }

        public string DisplayText
        {
            get
            {
                return BodyText;
            }
        }

        public string BodyText { get; set; }

        public Uri Url { get; set; }        
    }
}
