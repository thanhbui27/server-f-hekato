using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.VnpayLb;
using DoAn.Repositories.Order;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.VNPAY;

namespace DoAn.Repositories.VNPAY
{
    public class VnpayRepositories : IVnpayRepositories
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepositories _IOrderRepositories;
        public VnpayRepositories(IConfiguration configuration, IOrderRepositories orderRepositories)
        {
            _configuration = configuration;
            _IOrderRepositories = orderRepositories;
        }
        public async Task<ApiResult<string>> CreatePaymentUrl(CreateOrders model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            var create = await _IOrderRepositories.create(model);

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.total * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{create.Data}-{model.users.fullName}-{model.total}");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return new ApiSuccessResult<string>(paymentUrl);
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            if(response.Success)
            {
                string[] order = response.OrderDescription.Split("-");
                _IOrderRepositories.updateTransCode(Int32.Parse(order[0]),response.TransactionId);

            }
            return response;
        }
    }
}
