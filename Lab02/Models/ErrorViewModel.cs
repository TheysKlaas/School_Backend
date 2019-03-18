using System;
using System.Net;

namespace School_Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public HttpStatusCode HttpStatuscode { get; set; }
    }
}