using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuyPamojaMpesaIntegration.Models
{
    public class MpesaStkModel
    {
        public class MpesaStkPushModel
        {
            public int Id { get; set; }
            public stkCallbackobj Body { get; set; }
        }
        public class stkCallbackobj
        {
            public int Id { get; set; }
            public stkCallbackcontent stkCallback { get; set; }
        }

        public class stkCallbackcontent
        {
            public int Id { get; set; }
            public string MerchantRequestID { get; set; }
            public string CheckoutRequestID { get; set; }
            public string ResultCode { get; set; }
            public string ResultDesc { get; set; }

            [Display(Name = "Body")]
            public string stkCallback { get; set; }

            [Display(Name = "Amount ")]
            public string Amount { get; set; }

            [Display(Name = "MpesaReceiptNumber")]
            public string MpesaReceiptNumber { get; set; }

            [Display(Name = "TransactionDate")]
            public string TransactionDate { get; set; }
            [Display(Name = "User Email")]
            public string UserEmail { get; set; }
            public string Course { get; set; }



            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }


            public CallbackMetadataobj CallbackMetadata { get; set; }

        }

        public class ItemList
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }




        }
        //CallbackMetadata



        public class CallbackMetadataobj
        {
            public int Id { get; set; }
            //public CallbackMetadatcontent Item { get; set; }
            public List<ItemList> Item { get; set; }



        }

        public class CallbackMetadatcontent
        {
            public int Id { get; set; }
            public List<ItemList> itemLists { get; set; }
        }
    }
}
