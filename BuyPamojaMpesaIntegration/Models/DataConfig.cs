using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BuyPamojaMpesaIntegration.Models
{
    public class DataConfig
    {
       public static string CallbackUrl = ConfigurationManager.AppSettings["CallbackUrl"];
       public static string MailFromAddress = ConfigurationManager.AppSettings["MailFromAddress"];
       public static string MailUsername = ConfigurationManager.AppSettings["MailUsername"];
       public static string MailDisplayName = ConfigurationManager.AppSettings["MailDisplayName"];
       public static string MailPassword = ConfigurationManager.AppSettings["MailPassword"];
       public static string MailServer = ConfigurationManager.AppSettings["MailServer"];
       public static string MailPort = ConfigurationManager.AppSettings["MailPort"];

    }
}