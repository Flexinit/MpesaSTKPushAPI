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

    }
}