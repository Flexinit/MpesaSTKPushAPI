using IESR_PAYMENTS;
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using BuyPamojaMpesaIntegration.Models;
using System.Web.Http;

namespace BuyPamojaMpesaIntegration.Controllers
{

    public class MpesaStkController : Controller
    {

        
        public ActionResult MakeStkPayment(STKPushRequest sTKPushRequest)
        {
            Logger.WriteLog(DateTime.Now + "STK****PUSH***REQUEST: MSISDN "+sTKPushRequest.PhoneNumber+ " Amount: "+ sTKPushRequest.Amount+ " PartyB :"+sTKPushRequest.PartyB);


            string ab = "https://api.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
            string baseUrl = ab;

            string user = "FIL8zXh4XEWMZ1fFM93FisW3IO2UyJTH23GHJ9IgPoWYF1kU";
            string pass = "9RSPnmGzt9OmrZ0evGc8RfcoAZSBdJktxHWmVARrf42fxzw92sS8IRCpdYAzVdqD";
            string shortCode = "864964";
            string PartyB = '"' + sTKPushRequest.PartyB + '"'; // Till number - 355999
            string businessshortcode = '"' + "864964" + '"';
            string passkey = "2c86884febaff4521d7211aff416a7e421e6e95734032f40e26d85d25cc5c368";

            
            string phonenumber2 = '"' + "254" + sTKPushRequest.PhoneNumber.Substring(1) + '"';
            string ttype = '"' + "CustomerBuyGoodsOnline" + '"';
            string amt = '"' + sTKPushRequest.Amount + '"';
            string accountref = '"' + "MOI'S BRIDGE QUARRY CO LTD " + '"';
            string tdesc = '"' + sTKPushRequest.AccountName + '"';

          

            string callback = '"' + DataConfig.CallbackUrl + '"';
            var timeStamp = GetTimestamp(DateTime.Now);
            string newtimestamp = '"' + timeStamp + '"';
            ////////////////

            
            var newpassword = Base64Encode(shortCode, passkey, timeStamp);
            string currentpassword = '"' + newpassword + '"';
   
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                String token =  GenerateAccessToken.generate(user,pass);
            string resp_token = ((JObject.Parse(token))["access_token"]).ToString();

            request.Headers.Add("authorization", "Bearer " + resp_token);
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                request.KeepAlive = false;
                request.Method = "POST";


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                string json = "{\"BusinessShortCode\":" + businessshortcode + "," +
                          "\"Password\":" + currentpassword + "," +
                          "\"Timestamp\":" + newtimestamp + "," +
                          "\"TransactionType\":" + ttype + "," +
                          "\"Amount\":" + amt + "," +
                          "\"PartyA\":" + phonenumber2 + "," +
                          "\"PartyB\":" + PartyB + "," +
                          "\"PhoneNumber\":" + phonenumber2 + "," +
                          "\"CallBackURL\":" + callback + "," +
                          "\"AccountReference\":" + accountref + "," +
                          "\"TransactionDesc\":" + tdesc + "}";

                streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // Get the stream associated with the response.
                    Stream receiveStream = response.GetResponseStream();
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                string resp = readStream.ReadToEnd().ToString();

                    Console.WriteLine(readStream.ReadToEnd());
                    response.Close();
                    readStream.Close();

                Logger.WriteLog(DateTime.Now + ":  Response To Main Server: " + resp);

                return Json(new { Status = "OK", Message = resp }, JsonRequestBehavior.AllowGet);
            }

             
                catch (WebException ex)
                {
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    Console.WriteLine(resp);

                Logger.WriteLog(DateTime.Now + ":  Response To Main Server: " + resp);

                return Json(new { Status = "OK", Message = DateTime.Now + ":  Response To Main Server: " + resp },JsonRequestBehavior.AllowGet);
            }
          
        }

    
        
          
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Encode(string shortcode, string passkey, string timestamp)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(shortcode + passkey + timestamp);
            return System.Convert.ToBase64String(plainTextBytes);
        }


    }

}