namespace SmartIBS.Models
{
    public class BuyerInfo
    {
        public string VRN { get; set; } = "";
        public string TIN { get; set; } = "";
        public string Line3 { get; set; } = "";
        public string Line2 { get; set; } = "";
        public string Line1 { get; set; } = "";
        public double Balance { get; set; }
    }
}