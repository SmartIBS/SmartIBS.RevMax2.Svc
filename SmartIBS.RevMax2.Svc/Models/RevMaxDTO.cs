using System;
using System.Collections.Generic;
using System.Text;

namespace SmartIBS.Models
{
    public class RevMaxDTO
    {
        public string InvoiceNumber { get; set; }
        public string Cashier { get; set; }
        public List<SlipData> LineItems { get; set; }

        public PaymentInfo PaymentInfo { get; set; }
        public BuyerInfo BuyerInfo { get; set; }
    }
}
