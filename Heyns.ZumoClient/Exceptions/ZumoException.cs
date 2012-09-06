using System;
using System.Net;

namespace Heyns.ZumoClient
{
    public class ZumoException : Exception
    {
        public HttpStatusCode HttpStatus { get; internal set; }

        public ZumoException(string message, HttpStatusCode status)
            :base(message)
        {
            this.HttpStatus = status;
        }
    }
}
