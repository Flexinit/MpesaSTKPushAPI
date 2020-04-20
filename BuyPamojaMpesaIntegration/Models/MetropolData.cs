using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyPamojaMpesaIntegration.Models
{
    public class MetropolData
    {

        public int report_type { get; set; }
        public string identity_number { get; set; }
        public int report_reason { get; set; }
        public int loan_amount { get; set; }
        public string identity_type { get; set; }
       
    }
}