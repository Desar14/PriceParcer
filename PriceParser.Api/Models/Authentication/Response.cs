using System.Collections;

namespace PriceParser.Api.Models.Authentication
{
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IEnumerable Description { get; set; }
    }
}
