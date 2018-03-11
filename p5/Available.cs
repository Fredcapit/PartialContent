using p5.interfaces;
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
using System.Threading.Tasks;
using p5.Extensions;

namespace p5
{
    public class PartialContent : PartialContentProducerInterface
    {
               

        public void ProduceContent()
        {
            HttpWebRequest req=new HttpWebRequest();
            
            throw new System.NotImplementedException();
        }
    }
}