using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using p5.Extensions;

namespace p5
{
   public class StreamVideo
    {
        private HttpRequestMessage _req;
        private HttpResponseMessage _res;
        private long _fileLen;
        private MediaTypeHeaderValue _mediaType;
        private object locker = new object();
        private bool DataAvailable = true;
        private bool WriteAvalable = true;
        private Queue<byte[]> buffer = new Queue<byte[]>();
        private int ReadBytesLength = 1024;

        public StreamVideo(HttpRequestMessage req, HttpResponseMessage res,MediaTypeHeaderValue mediaType)
        {
            _req = req;
            _res = res;
            _mediaType = mediaType;
  
        }
       
        public bool IsSatisfaible()
        {
            if (!_req.Headers.IsRangeHeaderExist())
            { ProduceSimpleResponse(); return false; }

            if (!_req.Headers.IsRangeHeaderCorrect(_fileLen))
            { NotSatisfaibleResponseMessage(); return false; }

                return true;
  
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
            _res.Content.Headers.ContentType = _mediaType;

            
        }
        private void ProduceStream(Stream input, Stream output)
        {
            while (DataAvailable || WriteAvalable)
            {
                lock (locker)
                {
                    Read(input);
                }
                lock (locker)
                {
                    Write(output);
                }

            }
        }
        private void Read(Stream input)
        {

            byte[] readbuffer = new byte[ReadBytesLength];
            int bytesread = 0;
            try
            {
                bytesread = input.Read(readbuffer, 0, ReadBytesLength);
            }
            finally
            {
                DataAvailable = (bytesread > 0);
            }
            if (DataAvailable)
            {
                byte[] factbuffer = new byte[bytesread];
                Buffer.BlockCopy(readbuffer, 0, factbuffer, 0, bytesread);
                buffer.Enqueue(factbuffer);
            }

        }
        private void Write(Stream output)
        {
            try
            {
                byte[] bytesforWriting = buffer.Dequeue();
                output.Write(bytesforWriting, 0, bytesforWriting.Length);

            }
            catch
            {
                WriteAvalable = false;
            }
        }
    }
}
