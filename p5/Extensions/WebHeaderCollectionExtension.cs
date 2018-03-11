

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
        public static bool IsRangeHeaderCorrect (this WebHeaderCollection headerCollection, long fileLen)
        {
            RangeHeaderValue rangeHeader = new RangeHeaderValue();
            if (!RangeHeaderValue
                .TryParse(headerCollection[HttpRequestHeader.Range], out rangeHeader))
                return false;
            bool unitIsNotbytes=rangeHeader.Unit !="bytes";
            bool multipleRanges = rangeHeader.Ranges.Count>1;
            RangeItemHeaderValue range=rangeHeader.Ranges.First();
            bool start_end_fileLen_error=!(range.RangeHeaderStart<fileLen && range.RangeHeaderEnd<fileLen);
            return !(unitIsNotbytes || multipleRanges || start_end_fileLen_error);

        }
        private static long RangeHeaderStart (this RangeItemHeaderValue range, long fileLen)
         {
            var start=0;
            start=(range.From !=null)?range.From.Value:((range.To!=null)?fileLen-range.To.Value:start);
            return start;
         }
        private static long RangeHeaderEnd (this RangeItemHeaderValue range, long fileLen)
         {
            var start=0;
            start=(range.From !=null)?(range.To !=null ? range.To.Value: fileLen-1):(fileLen-1);
            return start;
         }
    }
}

 