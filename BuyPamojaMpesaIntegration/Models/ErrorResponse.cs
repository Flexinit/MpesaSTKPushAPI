using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyPamojaMpesaIntegration.Models
{
    public class ErrorResponse
    {

       
            public string requestId { get; set; }
            public string errorCode { get; set; }
            public string errorMessage { get; set; }
        

    }
}