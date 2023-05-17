using System;

namespace SmartIBS.Models
{
    public class PaymentInfo
    {
        public string PaidMode { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string ResponseCode { get; set; } = "";
        public string RetrievalRef { get; set; } = "";
        public double Amount { get; set; } = 0;
        public string TransactionId { get; set; } = "";
        public string TerminalId { get; set; } = "";
        public string ActionCode { get; set; } = "";
        public string EntryMode { get; set; } = "";
        public double Discount { get; set; }
        public double Tax { get; set; }
    }
}