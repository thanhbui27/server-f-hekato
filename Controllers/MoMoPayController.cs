using DoAn.Helpers.MoMoPayLb;
using DoAn.Repositories.MoMo;
using DoAn.Repositories.VNPAY;
using DoAn.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoMoPayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMoMoRepositories _moMoRepositories;

        public MoMoPayController(IConfiguration configuration, IMoMoRepositories moMoRepositories)
        {
            _configuration= configuration;
            _moMoRepositories= moMoRepositories;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePaymentUrl(CreateOrders model)
        {
            var res = await _moMoRepositories.CreatePaymentUrl(model,HttpContext);
            return Ok(res);

        }

        [HttpGet("ConfirmPaymentClient")]
        public async Task<IActionResult> ConfirmPaymentClient()
        {
            var response = _moMoRepositories.PaymentExecute(Request.Query);        
            if (response.Success && response.ResponseCode.Contains("0"))
            {
                return Redirect($"{_configuration["ClientUrl"]}/orderCompleted?success={response.Success}");
            }
            return Redirect($"{_configuration["ClientUrl"]}/orderCompleted?success=false");

        }

    }
}
