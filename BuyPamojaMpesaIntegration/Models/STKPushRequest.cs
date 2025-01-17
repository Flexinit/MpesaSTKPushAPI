using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BuyPamojaMpesaIntegration.Models
{
    public class STKPushRequest
    {
        public string PhoneNumber { get; set; }
        public string Amount { get; set; }
        public string AccountName { get; set; }
        public string PartyB { get; set; }
    }
}