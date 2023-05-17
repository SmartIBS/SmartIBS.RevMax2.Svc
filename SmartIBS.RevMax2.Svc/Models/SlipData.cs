namespace SmartIBS.Models
{
    public class SlipData
    {
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; } = 1;
        public string Unit { get; set; } = "-";
        public string TaxCode { get; set; }
        public double Discount { get; set; }
        public int LineID { get; set; }
        public string ItemCode { get; set; }
        public double Tax { get;  set; }
        public double TaxRate { get; set; }
        public double LineTotal { get; set; }
    }
}