using BuyPamojaMpesaIntegration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IESR_PAYMENTS.Controllers
{
    public class GenerateUrlsController : Controller
    {
        // GET: GenerateAuthorizationKey
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Generate()
        {
            try
            {
                //SANDBOX CREDENTIALS
                string token = GenerateAccessToken.generate("FIL8zXh4XEWMZ1fFM93FisW3IO2UyJTH23GHJ9IgPoWYF1kU",
                    "9RSPnmGzt9OmrZ0evGc8RfcoAZSBdJktxHWmVARrf42fxzw92sS8IRCpdYAzVdqD");
                string resp_token = ((JObject.Parse(token))["access_token"]).ToString();

                //string baseUrl = "https://api.safaricom.co.ke/mpesa/c2b/v1/registerurl";
                string baseUrl = "https://api.safaricom.co.ke/mpesa/c2b/v2/registerurl";

                string validationURL = '"' + "https://suamapps.com:54330/Account/ValidateNewPay" + '"';
                string confirmationURL = '"' + "https://suamapps.com:54330/Account/NewPayment" + '"';
                string responseType = '"' + "Completed" + '"';                
                string businessshortcode = '"' + "865964" + '"'; // Store Number
                
                // return resp_token;         

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

                request.Headers.Add("Authorization", "Bearer " + resp_token);
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");

                request.KeepAlive = false;
                request.Method = "POST";


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                   
                    string json = "{\"ShortCode\":" + businessshortcode + "," +
                       "\"ResponseType\":" + responseType + "," +
                       "\"ValidationURL\":" + validationURL + "," +
                       "\"ConfirmationURL\":" + confirmationURL + "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
          
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // Get the stream associated with the response.
                    Stream receiveStream = response.GetResponseStream();
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var resp = readStream.ReadToEnd();
                    Console.WriteLine(resp);
                    response.Close();
                    readStream.Close();

                return Json(new { response = resp, } ,JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Logger.WriteLog("Exception " + ex.InnerException);
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public string test()
        {
            return "hello sir";
        }


      

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffffff");
        }
        // 

        private static string SHA256HexHashString(string StringIn)
        {
            string hashString;
            using (var sha256 = SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(StringIn));
                hashString = ToHex(hash, false);
            }

            return hashString;
        }


        private static string ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }

       public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }



                return builder.ToString();
            }
        }


        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }


        [HttpPost]
        public string ReceivePay(string username, string password)
        {
            return "hello sir, "+username+", "+password;
        }
    }
}