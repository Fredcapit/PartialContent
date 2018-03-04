using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web.Configuration;
using System.Web.Http;

namespace p5
{
    public class PartialContent : PartialContentProducerInerface
    {
        public Task CanRangeBeFulfilled()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> isAvailable()
        {

        }

        public Task IsRangeHeaderExist(WebRequest req)
        {
            RangeHeaderValue rangeHeader = req.Headers.R;
            if (rangeHeader == null || !rangeHeader.Ranges.Any())
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new PushStreamContent((outputStream, httpContent, transpContext)
                =>
                    {
                        using (outputStream) // Copy the file to output stream straightforward. 
                using (Stream inputStream = fileInfo.OpenRead())
                        {
                            try
                            {
                                inputStream.CopyTo(outputStream, ReadStreamBufferSize);
                            }
                            catch (Exception error)
                            {
                                Debug.WriteLine(error);
                            }
                        }
                    }, GetMimeNameFromExt(fileInfo.Extension));

                response.Content.Headers.ContentLength = totalLength;
                return response;
            }
        }

        public Task ProduceConent()
        {
            throw new System.NotImplementedException();
        }
    }
}