using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using p5.Extensions;

namespace p5
{
   public class StreamVideo
    {
        private HttpRequestMessage _req;
        private HttpResponseMessage _res;
        private long _fileLen;
        private string _filename;
        private string _filenameEx;
        // We have a read-only dictionary for mapping file extensions and MIME names. 
        public readonly IReadOnlyDictionary<string, string> MimeNames;

        public StreamVideo(HttpRequestMessage req, HttpResponseMessage res, string filename)
        {
            _req = req;
            _res = res;
            var mimeNames = new Dictionary<string, string>();

            mimeNames.Add(".mp3", "audio/mpeg");    // List all supported media types; 
            mimeNames.Add(".mp4", "video/mp4");
            mimeNames.Add(".ogg", "application/ogg");
            mimeNames.Add(".ogv", "video/ogg");
            mimeNames.Add(".oga", "audio/ogg");
            mimeNames.Add(".wav", "audio/x-wav");
            mimeNames.Add(".webm", "video/webm");

            MimeNames = new ReadOnlyDictionary<string, string>(mimeNames);
        }
        public void ProduceContent()
        {
            if (!_req.Headers.IsRangeHeaderExist())
            { ProduceSimpleResponse(); goto Finish; }

            if (!_req.Headers.IsRangeHeaderCorrect(_fileLen))
            { NotSatisfaibleResponseMessage(); goto Finish; }

            Finish:
            int result = 0;
        }
        private void ProduceSimpleResponse()
        {
            _res.StatusCode = HttpStatusCode.OK;
        }
        private void NotSatisfaibleResponseMessage()
        {
            _res.StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable;
            _res.Content = new StreamContent(Stream.Null);  // No content for this status.
            _res.Content.Headers.ContentRange = new ContentRangeHeaderValue(_fileLen);
            _res.Content.Headers.ContentType = GetMimeNameFromExt(_filenameEx);

            
        }
        private MediaTypeHeaderValue GetMimeNameFromExt(string ext)
        {
            string value;

            if (MimeNames.TryGetValue(ext.ToLowerInvariant(), out value))
                return new MediaTypeHeaderValue(value);
            else
                return new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        }
    }
}
