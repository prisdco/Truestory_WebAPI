using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Infrastructure.Exceptions
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public readonly HttpResponseMessage httpResponseMessage;
        public HttpResponseException()
        {
        }

        public HttpResponseException(HttpResponseMessage httpResponseMessage)
        {
            this.httpResponseMessage = httpResponseMessage;
        }

        public HttpResponseException(string message) : base(message)
        {
        }


        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
