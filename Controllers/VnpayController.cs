using DoAn.Repositories.Order;
using DoAn.Repositories.VNPAY;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.VNPAY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VnpayController : Controller
    {
        private readonly IVnpayRepositories _vnpayRepositories;
        private readonly IConfiguration _configuration;

        public VnpayController(IVnpayRepositories vnpayRepositories, IConfiguration configuration)
        {
            _vnpayRepositories = vnpayRepositories;
            _configuration = configuration;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePaymentUrl(CreateOrders model)
        {
            var url = await _vnpayRepositories.CreatePaymentUrl(model, HttpContext);

            return Ok(url);
        }

        [HttpGet("paymentCallBack")]
        public IActionResult PaymentCallback()
        {
            var response = _vnpayRepositories.PaymentExecute(Request.Query);
            if(response.Success && response.ResponseCode.Contains("00"))
            {   
                return Redirect($"{_configuration["ClientUrl"]}/orderCompleted?success={response.Success}");
            }
            return Redirect($"{_configuration["ClientUrl"]}/orderCompleted?success=false");
        }
    }
}
