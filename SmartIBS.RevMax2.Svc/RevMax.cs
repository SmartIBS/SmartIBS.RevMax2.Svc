using SmartIBS.Models;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SmartIBS.RevMax2.Svc
{
    public static class RevMax
    {
        private static RevmaxAPI.Revmaxlib revmaxlib = new RevmaxAPI.Revmaxlib();

        public static string GetDeviceDetail()
        {
            try
            {
                var info = revmaxlib.GetCardDetails();
                if (info == null)
                    return "RevMax Device Error";

                var dev = new XmlDocument();
                dev.LoadXml(info);

                var code = dev?.DocumentElement?.SelectSingleNode("/Response/Code");

                if (code == null || code.InnerText == "0") return "RevMax Device Error";

                var data = dev?.DocumentElement?.SelectSingleNode("/Response/Data");

                StringBuilder sb = new StringBuilder("{ ");
                sb.AppendLine("\"COMPANYNAME\":\"" + data?.SelectSingleNode("COMPANYNAME")?.InnerText+"\",");
                sb.AppendLine("\"ADDRESS\":\""+data?.SelectSingleNode("ADDRESS")?.InnerText + "\",");
                sb.AppendLine("\"BPN\":\"" + data?.SelectSingleNode("BPN")?.InnerText + "\",");
                sb.AppendLine("\"VAT\":\"" + data?.SelectSingleNode("VAT")?.InnerText + "\",");
                //sb.AppendLine();
                sb.AppendLine("\"Serial\":\"" + data?.SelectSingleNode("SERIALNUMBER")?.InnerText + "\"");

                if(IsActive())
                {
                    sb.AppendLine(",");

                    var lic = LicenseData();
                    sb.AppendLine("\"License\":\"" + lic?.SelectSingleNode("End")?.InnerText + "\",");
                    sb.AppendLine("\"Expiry\":\"" + lic?.SelectSingleNode("End")?.InnerText + "\",");
                }
                
                sb.AppendLine("}");
                

                return sb.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static XmlNode LicenseData()
        {
            var lic = new XmlDocument();
            lic.LoadXml(
                revmaxlib.GetLicense());

            XmlNode res = lic.DocumentElement.SelectSingleNode("/Response/Data");          

            return res;
        }


        public static string ZedReport()
        {
            return revmaxlib.ZReport();
        }
        public static bool IsActive()
        {
            var lic = new XmlDocument();
            lic.LoadXml(
                revmaxlib.GetLicense());

            XmlNode res = lic.DocumentElement.SelectSingleNode("/Response/Code");

            if (res == null || res.InnerText == "0") {
                lic.LoadXml( // read license from config
                    revmaxlib.SetLicense("TO6Jm9eDgonPfQtcDjc56LfMQV6ne1s9H91EdGiTQD2EbiIS79TEIzENfzT0IQag"));

                res = lic.DocumentElement.SelectSingleNode("/Response/Code");
            }

            if (res == null) return false;

            return res.InnerText == "1";
            
        }

        /// <summary>
        /// Set revMax Key
        /// </summary>
        /// <param name="key">Serial obtained from service provider</param>
        /// <returns></returns>
        public static XmlNode SetKey(string key)
        {
            var lic = new XmlDocument();

            // Set key and read XMl response
            lic.LoadXml(
                revmaxlib.SetLicense(key));

            //XmlNode? res = lic?.DocumentElement?.SelectSingleNode("/Response/Code");

           // string? status = "Failed"; // default to failed status

            // No need to proceeed of response is null
            //if (res == null) return status;


            // get status from Xml response
           // status = lic?.DocumentElement?.SelectSingleNode("/Response/Data/Status")?.InnerText;

            // return status as json object
            return lic?.DocumentElement?.SelectSingleNode("/Response/Data");
        }


        public static string Sign(RevMaxDTO receipt)
        {
            // var logPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\SmartIBS\\logs\\RevMax";

            try
            {
 
                // var deviceInfo = Program._revmax.GetCardDetails();0772373727
                var xEle = new XElement("ITEMS",
                                from item in receipt.LineItems
                                select new XElement("ITEM",
                                             new XElement("HH", item.LineID),
                                               new XElement("ITEMCODE", item.ItemCode),
                                               new XElement("ITEMNAME1", item.Description1),
                                               new XElement("ITEMNAME2", string.IsNullOrEmpty(item.Description2) ? "N/A" : item.Description2),
                                               new XElement("QTY", item.Quantity),
                                               new XElement("PRICE", item.Price),
                                               new XElement("AMT", item.LineTotal),
                                               new XElement("TAX", item.Tax),
                                               new XElement("TAXR", item.TaxRate)
                                           ));

                var res = revmaxlib.TransactM(
                    receipt.PaymentInfo.PaidMode,
                    receipt.PaymentInfo.ActionCode,
                    receipt.InvoiceNumber,
                    receipt.BuyerInfo.Line1,
                    receipt.BuyerInfo.TIN,
                    receipt.BuyerInfo.Line2,
                    receipt.BuyerInfo.Line3,
                    receipt.BuyerInfo.VRN,
                    receipt.PaymentInfo.Amount.ToString(),
                    receipt.PaymentInfo.Tax.ToString(), "01",
                    receipt.Cashier, "",
                    xEle.ToString(),
                    receipt.PaymentInfo.CardNumber);


                return res;

            }
            catch (Exception ex)
            {
                return $"ERROR,{ex.Message}";
            }

        }

    }
}
