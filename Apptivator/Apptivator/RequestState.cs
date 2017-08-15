using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apptivator
{
    public class RequestState
    {
        // This class stores the state of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] bufferRead;
        public WebRequest request;
        public WebResponse response;
        public Stream responseStream;

        public RequestState()
        {
            bufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            responseStream = null;
        }

        public async Task<string> Response(string weburi)
        {            

            WebRequest myWebRequest = WebRequest.Create(weburi);

            // Send the 'WebRequest' and wait for response.
            WebResponse myWebResponse = await myWebRequest.GetResponseAsync();

            // Obtain a 'Stream' object associated with the response object.
            Stream ReceiveStream = myWebResponse.GetResponseStream();

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            Console.WriteLine("\nResponse stream received");
            Char[] read = new Char[256];

            // Read 256 charcters at a time.    
            int count = readStream.Read(read, 0, 256);
            String str = "";
            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                str = new String(read, 0, count);
                Console.Write(str);
                count = readStream.Read(read, 0, 256);
            }
            
            readStream.Close();
            myWebResponse.Close();

            return str;

        }

    }

}
