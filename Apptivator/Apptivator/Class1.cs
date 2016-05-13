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
    }

    public class Class1
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;

        Rnd.Common.Utilities util = new Rnd.Common.Utilities();
        public static string LocalAppFolder = Path.Combine(new Rnd.Common.Utilities().LocalAppData, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        public static string LocalUpdateConfigurationFile = Path.Combine(LocalAppFolder, "activator.bin");

        public Class1()
        {
            Rnd.Common.Models.Activator act = new Rnd.Common.Models.Activator();
            act.ActivationCode = "12345";
            act.MacAddress = "F0-79-59-8E-05-DF";


            util.SerializeBinFile(@"C:\Users\J. Mon\Desktop\activation.bin", act);




            try
            {
                WebRequest myWebRequest = WebRequest.Create("http://webservice.intdesignservices.com/codeservice.php?code=NTBAMNQT9237341&mac=000B0E0FED");
                RequestState myRequestState = new RequestState();
                // The 'WebRequest' object is associated to the 'RequestState' object.
                myRequestState.request = myWebRequest;
                // Start the Asynchronous call for response.
                IAsyncResult asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
                allDone.WaitOne();
                // Release the WebResponse resource.
                myRequestState.response.Close();
                Console.Read();
            }
            catch (WebException e)
            {
                Console.WriteLine("WebException raised!");
                Console.WriteLine("\n{0}", e.Message);
                Console.WriteLine("\n{0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception raised!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }



        }

        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // Set the State of request to asynchronous.
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                WebRequest myWebRequest1 = myRequestState.request;
                // End the Asynchronous response.
                myRequestState.response = myWebRequest1.EndGetResponse(asynchronousResult);
                // Read the response into a 'Stream' object.
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.responseStream = responseStream;
                // Begin the reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

            }
            catch (WebException e)
            {
                Console.WriteLine("WebException raised!");
                Console.WriteLine("\n{0}", e.Message);
                Console.WriteLine("\n{0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception raised!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
        }
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // Result state is set to AsyncState.
                RequestState myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.responseStream;
                int read = responseStream.EndRead(asyncResult);
                // Read the contents of the HTML page and then print to the console.
                if (read > 0)
                {
                    myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                }
                else
                {
                    Console.WriteLine("\nThe HTML page Contents are:  ");
                    if (myRequestState.requestData.Length > 1)
                    {
                        string sringContent;
                        sringContent = myRequestState.requestData.ToString();
                        Console.WriteLine(sringContent);
                    }
                    Console.WriteLine("\nPress 'Enter' key to continue........");
                    responseStream.Close();
                    allDone.Set();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("WebException raised!");
                Console.WriteLine("\n{0}", e.Message);
                Console.WriteLine("\n{0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception raised!");
                Console.WriteLine("Source : {0}", e.Source);
                Console.WriteLine("Message : {0}", e.Message);
            }

        }

    }
}
