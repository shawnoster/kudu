using System;
using System.Net;

namespace Librarian
{
    public class RestRequestException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public string Content { get; private set; }

        public RestRequestException()
        {
        }

        public RestRequestException(string message)
            : this(message, (Exception)null)
        {
        }

        public RestRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RestRequestException(string message, HttpStatusCode status)
            : this(message, (Exception) null, status, null)
        {
        }

        public RestRequestException(string message, HttpStatusCode status, string content)
            : this(message, (Exception)null, status, content)
        {
        }

        public RestRequestException(string message, Exception innerException, HttpStatusCode status, string content)
            : base(message, innerException)
        {
            this.Status = status;
            this.Content = content;
        }
    }
}
