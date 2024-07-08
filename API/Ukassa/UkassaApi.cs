using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WizardLibrary.API.Ukassa
{
    public class UKassaApi
    {
        protected static HttpClient Client { get; set; }
        protected static string ShopId { get; set; }
        protected static string SecretKey { get; set; }
        public UKassaApi(string shopId, string secretKey)
        {
            Client = new HttpClient();
            ShopId = shopId;
            SecretKey = secretKey;
        }
    }
    public class UCreatePayment : UKassaApi
    {
        public ResponsePaymentJson response { get; set; }
        public UCreatePayment(double amountValue, string return_url, string currency = "RUB", string description = "Покупка") : base(ShopId, SecretKey)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(
                new RequestPaymentJson
                {
                    amount = new() { currency = currency, value = $"{amountValue}" },
                    capture = true,
                    confirmation = new() { type = "redirect", return_url = return_url },
                    description = description
                }), Encoding.UTF8, "application/json"); ;

            var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ShopId}:{SecretKey}"));
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
            string idempotenceKey = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add("Idempotence-Key", idempotenceKey);
            HttpResponseMessage resp = client.PostAsync("https://api.yookassa.ru/v3/payments", httpContent).Result;
            string responseContent = resp.Content.ReadAsStringAsync().Result;
            response = JsonConvert.DeserializeObject<ResponsePaymentJson>(responseContent);
        }
        public class RequestPaymentJson
        {
            public Amount amount { get; set; }
            public bool capture { get; set; }
            public ConfirmationRequest confirmation { get; set; }
            public string description { get; set; }
        }
        public class ResponsePaymentJson
        {
            public string id { get; set; }
            public string status { get; set; }
            public bool paid { get; set; }
            public Amount amount { get; set; }
            public ConfirmationResponese confirmation { get; set; }
            public DateTime created_at { get; set; }
            public string description { get; set; }
            //public Metadata metadata { get; set; }
            public Recipient recipient { get; set; }
            public bool refundable { get; set; }
            public bool test { get; set; }
        }
        public class Amount
        {
            public string value { get; set; }
            public string currency { get; set; }
        }
        public class Confirmation
        {
            public string type { get; set; }
        }
        public class ConfirmationResponese : Confirmation
        {
            public string confirmation_url { get; set; }
        }
        public class ConfirmationRequest : Confirmation
        {
            public string return_url { get; set; }
        }
        public class Recipient
        {
            public string account_id { get; set; }
            public string gateway_id { get; set; }
        }

    }

    internal class UCheckPayment : UKassaApi
    {
        public Payment response { get; set; }
        public UCheckPayment(string payment_id) : base(ShopId, SecretKey)
        {
            using HttpClient client = new();
            var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ShopId}:{SecretKey}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
            var resp = client.GetAsync($"https://api.yookassa.ru/v3/payments/{payment_id}").Result;
            string responseContent = resp.Content.ReadAsStringAsync().Result;
            response = JsonConvert.DeserializeObject<Payment>(responseContent);
        }
        public class Payment
        {
            public string id { get; set; }
            public string status { get; set; }
            public bool paid { get; set; }
            public Amount amount { get; set; }
            public Authorization_Details authorization_details { get; set; }
            public DateTime created_at { get; set; }
            public string description { get; set; }
            public DateTime expires_at { get; set; }
            public Payment_Method payment_method { get; set; }
            public Recipient recipient { get; set; }
            public bool refundable { get; set; }
            public bool test { get; set; }
            public Income_Amount income_amount { get; set; }
        }
        public class Amount
        {
            public string value { get; set; }
            public string currency { get; set; }
        }
        public class Authorization_Details
        {
            public string rrn { get; set; }
            public string auth_code { get; set; }
            public Three_D_Secure three_d_secure { get; set; }
        }
        public class Three_D_Secure
        {
            public bool applied { get; set; }
        }
        public class Payment_Method
        {
            public string type { get; set; }
            public string id { get; set; }
            public bool saved { get; set; }
            public Card card { get; set; }
            public string title { get; set; }
        }
        public class Card
        {
            public string first6 { get; set; }
            public string last4 { get; set; }
            public string expiry_month { get; set; }
            public string expiry_year { get; set; }
            public string card_type { get; set; }
            public Card_Product card_product { get; set; }
            public string issuer_country { get; set; }
            public string issuer_name { get; set; }
        }
        public class Card_Product
        {
            public string code { get; set; }
            public string name { get; set; }
        }
        public class Recipient
        {
            public string account_id { get; set; }
            public string gateway_id { get; set; }
        }
        public class Income_Amount
        {
            public string value { get; set; }
            public string currency { get; set; }
        }
    }
}
