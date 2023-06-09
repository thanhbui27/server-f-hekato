using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.MoMoPayLb;
using DoAn.Helpers.VnpayLb;
using DoAn.Repositories.Order;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.VNPAY;
using Newtonsoft.Json.Linq;

namespace DoAn.Repositories.MoMo
{
    public class MoMoRepositories : IMoMoRepositories
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepositories _IOrderRepositories;
        public MoMoRepositories(IConfiguration configuration, IOrderRepositories orderRepositories)
        {
            _configuration = configuration;
            _IOrderRepositories = orderRepositories;
        }
        public async Task<ApiResult<string>> CreatePaymentUrl(CreateOrders model, HttpContext context)
        {

            string endpoint = _configuration["MoMo:apiEndpoint"];
            string partnerCode = _configuration["MoMo:partnentCode"];
            string accessKey = _configuration["MoMo:accessKey"];
            string serectkey = _configuration["MoMo:secretKey"];
            var create = await _IOrderRepositories.create(model);
            string orderInfo = $"{create.Data}-{model.users.fullName}-{model.total}";
            string returnUrl = "https://localhost:7263/api/MoMoPay/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/api/MoMoPay/SavePayment"; 

            string amount = model.total.ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = create.Data.ToString();

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoPayLibrary crypto = new MoMoPayLibrary();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = crypto.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return new ApiSuccessResult<string>(jmessage.GetValue("payUrl").ToString());
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {

            var pay = new MoMoPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["MoMo:secretKey"]);
            if (response.Success && response.ResponseCode.Contains("0"))
            {
                string[] order = response.OrderDescription.Split("-");
                _IOrderRepositories.updateTransCode(Int32.Parse(order[0]), response.TransactionId);

            }else
            {
                string[] order = response.OrderDescription.Split("-");
                _IOrderRepositories.updateStatusOrder(Int32.Parse(order[0]),"canceled_payment");
            }
            return response;
        }
    }
}
