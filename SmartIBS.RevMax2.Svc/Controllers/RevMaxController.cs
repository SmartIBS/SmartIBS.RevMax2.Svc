using Microsoft.AspNetCore.Mvc;
using SmartIBS.Models;


namespace SmartIBS.RevMax2.Svc.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RevMaxController : ControllerBase
    {      

        private readonly ILogger<RevMaxController> _logger;
        private readonly IConfiguration _configuration;

        public RevMaxController(ILogger<RevMaxController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;

        }

        
        [HttpGet(Name = "DeviceInfo", Order =0)]
        public string DevoceInfo() => RevMax.GetDeviceDetail();

        [HttpPut(Name = "IsAlive", Order = 0)]
        public bool IsAlive([FromBody] string id) => RevMax.IsActive();


        [HttpGet(Name = "ZReport", Order =1)]
        public string ZReport()
        {
            try
            {
                return RevMax.revmaxlib.ZReport();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return $"ERROR,{ex.Message}";
            }


        }

        [HttpPost(Name = "Sign", Order = 2)]
        public string Sign([FromBody] RevMaxDTO receipt)
        {
            _logger.LogInformation($"{DateTime.Now:s}, Received -> {receipt.InvoiceNumber}, {receipt.PaymentInfo.Amount} {receipt.PaymentInfo.PaidMode} from {receipt.Cashier}");

            var res = RevMax.Sign(receipt);

            if (res.Split("#").Length < 3)
                _logger.LogInformation(res);
            else
            {
                res = $"OK,{res.Split("#")[2]}";
                _logger.LogInformation($"{DateTime.Now:s}, {receipt.InvoiceNumber} -> {res}");
            }

            return res;
        }

        [HttpPost(Name ="SetKey")]
        public IResult SetKey([FromBody] string key)
        {
            dynamic status = "FAILED";
            try
            {
                status = RevMax.SetKey(key);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }


            return Results.Ok(status);
        }
    }
}

