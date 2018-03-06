

using System.Net;
using System.Net.Http.Headers;
using System.Linq;

namespace p5.Extensions
{
    public static class WebheaderCollectionExtension
    {
        public static bool IsRangeHeaderExist(this WebHeaderCollection headerCollection)
        {
            RangeHeaderValue rangeHeader = new RangeHeaderValue();
            if (!RangeHeaderValue
                .TryParse(headerCollection[HttpRequestHeader.Range], out rangeHeader))
                return false;
            return (rangeHeader != null && rangeHeader.Ranges.Any());
        }
    }
}