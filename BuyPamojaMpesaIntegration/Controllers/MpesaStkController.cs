using IESR_PAYMENTS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using BuyPamojaMpesaIntegration.Models;

namespace BuyPamojaMpesaIntegration.Controllers
{
    public class MpesaStkController : Controller
    {
        public static string PhoneIMEI;
        public static string TransactnID;
     
            public ActionResult makeStkPayment(string phoneNumber, string amount, string deviceIMEI, string transID)
            {
            ////////////////
            PhoneIMEI = deviceIMEI;
            TransactnID = transID;
            string ab = "https://api.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
            string baseUrl = ab;
            string user = "CktydtNoEgUQ9H2Jx1IShtkY28G1Bg6V";
            string pass = "zo1MIYc8i2FKvg20";
          //  string phonenumber = '"' + "254" + phoneNumber + '"'; 
            string businessshortcode = '"' + "322116" + '"';
            //string phonenumber2 = '"' + "254" + "715049292" + '"';
            string phonenumber2 = '"' + "254" + phoneNumber.Substring(1) + '"';
            string ttype = '"' + "CustomerPayBillOnline" + '"';
            string amt = '"' + "1" + '"';
            string accountref = '"' + "EMESCCOS" + '"';
            string tdesc = '"' + "EMESCCOS" + '"';
            string callback = '"' + DataConfig.CallbackUrl + '"';
            var timeStamp = GetTimestamp(DateTime.Now);
            string newtimestamp = '"' + timeStamp + '"';
            ////////////////
           
            string shortCode = "322116";
            string passkey = "79b821652462bf5ac20c26c9407cd6a5e779a8e2db4710901373a2c544005eb3";
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
                          "\"PartyB\":" + businessshortcode + "," +
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