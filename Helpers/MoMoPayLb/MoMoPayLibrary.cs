using DoAn.ViewModels.VNPAY;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DoAn.Helpers.MoMoPayLb
{
    public class MoMoPayLibrary
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private readonly Dictionary<string, string> _responseData = new Dictionary<string, string>();
        public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var mmPay = new MoMoPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    mmPay.AddResponseData(key, value);
                }
            }

            var orderId = Convert.ToInt64(mmPay.GetResponseData("orderId"));
            var vnPayTranId = Convert.ToInt64(mmPay.GetResponseData("transId"));
            var responseTime = mmPay.GetResponseData("responseTime");
            var message = mmPay.GetResponseData("message");
            var localMessage = mmPay.GetResponseData("localMessage");
            var errorCode = mmPay.GetResponseData("errorCode"); 
            var vnpSecureHash =
                collection.FirstOrDefault(k => k.Key == "signature").Value; //hash của dữ liệu trả về
            var orderInfo = mmPay.GetResponseData("orderInfo");

            var checkSignature =
                mmPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature

            if (!checkSignature)
                return new PaymentResponseModel()
                {
                    Success = false
                };

            return new PaymentResponseModel()
            {
                Success = true,
                PaymentMethod = "momo",
                OrderDescription = orderInfo,
                OrderId = orderId.ToString(),
                PaymentId = vnPayTranId.ToString(),
                TransactionId = vnPayTranId.ToString(),
                Token = vnpSecureHash,
                ResponseCode = errorCode
            };
        }


        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = signSHA256(rspRaw, secretKey);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
          
            if (_responseData.ContainsKey("signature"))
            {
                _responseData.Remove("signature");
            }

            var sortedParams = _responseData.OrderBy(p => p.Key);

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlDecode(key) + "=" + WebUtility.UrlDecode(value) + "&");
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }

        public string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;

            }
        }
        public string getHash(string partnerCode, string merchantRefId,
            string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"amount\":" +
                amount + ",\"paymentCode\":\"" +
                paymentCode + "\",\"storeId\":\"" +
                storeId + "\",\"storeName\":\"" +
                storeName + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
            {
                try
                {
                    // MoMo's public key has format PEM.
                    // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                    rsa.FromXmlString(publicKeyXML);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }

        public string buildQueryHash(string partnerCode, string merchantRefId,
            string requestid, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"requestId\":\"" +
                requestid + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }

        public string buildRefundHash(string partnerCode, string merchantRefId,
            string momoTranId, long amount, string description, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"momoTransId\":\"" +
                momoTranId + "\",\"amount\":" +
                amount + ",\"description\":\"" +
                description + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }
     
        public string sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }


                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }

}
