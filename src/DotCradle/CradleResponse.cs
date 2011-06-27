using System.Collections.Generic;
using System.Net;

namespace DotCradle
{
    public class CradleResponse
    {
        public string Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}