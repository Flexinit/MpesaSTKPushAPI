using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace IESR_PAYMENTS
{
    public class GenerateAccessToken
    {
        public static string generate(string user, string pass)
        { 
            string myurl = "https://api.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

            string app_key = user;
            string app_secret = pass;

            byte[] auth = Encoding.UTF8.GetBytes(app_key + ":" + app_secret);
            string encoded = System.Convert.ToBase64String(auth);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myurl);
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.ContentType = "application/json";
            request.Headers.Add("cache-control", "no-cache");
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                var resp = readStream.ReadToEnd();
                Console.WriteLine(resp);
                response.Close();
                readStream.Close();
                return resp;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(resp);
                return resp;
            }
        }

        public static string generatebearerkey(string username, string password)
        {
            string _auth = string.Format("{0}:{1}", username, password);
            string _enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));
            return _enc;
        }
    }
}