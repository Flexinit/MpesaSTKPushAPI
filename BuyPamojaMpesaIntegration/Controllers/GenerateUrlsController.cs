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
                string token = GenerateAccessToken.generate("rOXDzQxciiMWISPianwzexdqkXw734Os", "HQ4mbKm8ZsTYqY73");
                string resp_token = ((JObject.Parse(token))["access_token"]).ToString();


                string baseUrl = "https://api.safaricom.co.ke/mpesa/c2b/v1/registerurl";

                //string myJson =
                //    "{'ShortCode': '777951'," +
                //    "'ResponseType': 'Completed'," +
                //    "'ValidationURL': 'http://safpay.buypamoja.com/Account/NewPayment\'," +
                //    "'ConfirmationURL':'https://safpay.buypamoja.com/Account/NewPayment\'}";
                //using (var client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + resp_token);
                //    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + resp_token);

                //    var responseData = await client.PostAsync(
                //        baseUrl,
                //         new StringContent(myJson, Encoding.UTF8, "application/json"));
                //}




                
                // return resp_token;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

                request.Headers.Add("authorization", "Bearer " + resp_token);
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");

                request.KeepAlive = true;
                request.Method = "POST";


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"ShortCode\":\"777951\"," +
                                  "\"ResponseType\":\"Completed\"," +
                                  "\"ValidationURL\":\"https://safpaybuypamoja.com/Account/NewPayment\"," +
                                  "\"ConfirmationURL\":\"https://safpaybuypamoja.com/Account/NewPayment\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


                //try
                //{
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // Get the stream associated with the response.
                    Stream receiveStream = response.GetResponseStream();

                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var resp = readStream.ReadToEnd();
                    Console.WriteLine(resp);
                    response.Close();
                    readStream.Close();

                return Json(new { response = resp, 
                    
                });

                //return Json(new { ShortCode = "883380",
                //    ResponseType = "",
                //    ValidationURL = @"http://payments.iesr.ac.ke/NewPayValidation/NewPay\",
                //    ConfirmationURL = @"http://payments.iesr.ac.ke/NewPayReceiption/NewPay\"
                //});



            }
            catch (Exception ex)
            {
                Logger.WriteLog("Exception " + ex.InnerException);
                return Json(new { error = ex.Message });
            }
        }

        public string test()
        {
            return "hello sir";
        }


        [HttpPost]
        public ActionResult QueryCRB() {

            try
            {
                string json = "{\"report_type\":\"5\"," +
                                "\"identity_number\":\"880000088\"," +
                                "\"identity_type\":\"001\"," +
                                "\"report_reason\":\"1\"," +
                                "\"loan_amount\":\"200\"}";

                MetropolData metropolData = new MetropolData();

                metropolData.report_type = 5;
                metropolData.identity_number = "880000088";
                metropolData.identity_type = "001";
                metropolData.report_reason = 1;
                metropolData.loan_amount = 200;

                string metropolJson = JsonConvert.SerializeObject(metropolData);


                //    string datePatt = @"M/d/yyyy hh:mm:ss tt";
                string datePatt = @"yyyyMMddhhmmsstt";
                string dateFormat = @"yyyyMMddHHmmssfffffffK";
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                String timeStamp = GetTimestamp(DateTime.UtcNow);



                DateTime saveUtcNow = DateTime.UtcNow;
                DateTime myDt;
                string dateString = saveUtcNow.ToString(datePatt);

                string OriginalString = "tOuyfPLXqjAfasGASRrzQnFFoEAqKD" +
                    json + 
                    "QrJkJVJUhhImPwjxNhBIQzgbPdoCNXCEQlqAIaurhFfyDQWrYZBOaHPrmGwa"+ timeStamp;


                string baseUrl = "https://api.metropol.co.ke:5555/v2_1/report/json";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);


                request.Headers.Add("X-METROPOL-REST-API-KEY", "tOuyfPLXqjAfasGASRrzQnFFoEAqKD");
                request.Headers.Add("X-METROPOL-REST-API-HASH", ComputeSha256Hash(OriginalString));
                request.Headers.Add("X-METROPOL-REST-API-TIMESTAMP", timeStamp);
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");

                request.KeepAlive = true;
                request.Method = "POST";



                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string jsonRequestData = "{\"report_type\":\"5\"," +
                                "\"identity_number\":\"880000088\"," +
                                "\"identity_type\":\"001\"," +
                                "\"report_reason\":\"1\"," +
                                "\"loan_amount\":\"200\"}";


                    streamWriter.Write(metropolJson);
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

                return Json(new
                {
                    Status = "Success",
                    data = resp



                });
            }

            catch (Exception ex)
            {
                Logger.WriteLog("Exception " + ex.InnerException);
                return Json(new { error = ex.Message
                });
            }

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