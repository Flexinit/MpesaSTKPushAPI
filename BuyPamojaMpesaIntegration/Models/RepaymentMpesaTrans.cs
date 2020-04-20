using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyPamojaMpesaIntegration.Models
{
    public class RepaymentMpesaTrans
    {
        public int ID { get; set; }
        public string DeviceIMEI { get; set; }
        public string TransactionID { get; set; }
        public string Amount { get; set; }
        public string MpesaReceiptNumber { get; set; }
        public string TransactionDate { get; set; }
        public string PhoneNumber { get; set; }
       
    }
}