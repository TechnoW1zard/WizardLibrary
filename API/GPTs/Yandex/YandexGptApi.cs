using System.Text;
using System.Text.Json;

namespace WizardLibrary.API.GPTs.Yandex
{
    public class YandexGptApi
    {
        /// <summary>
        /// Верефицирует пользователя, отправляя запрос с параметром OauthToken. Каждый раз возвращает новый IamToken   
        /// </summary>
        /// <param name="oauthToken">OauthToken который вы получили на сайте yandex</param>
        /// <returns>Возвращает IamToken, при ошибке выдаст null</returns>
        public static string Auth(string oauthToken)
        {
            string requestBody = $"{{\"yandexPassportOauthToken\":\"{oauthToken}\"}}";
            using HttpClient client = new HttpClient();
            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("https://iam.api.cloud.yandex.net/iam/v1/tokens", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseContent = response.Content.ReadAsStringAsync().Result;
                YandeAuthResponse auth = JsonSerializer.Deserialize<YandeAuthResponse>(responseContent);
                Console.WriteLine("ОБНОВЛЁН ТОКЕ IAM");
                Thread.Sleep(TimeSpan.FromHours(1));
                return auth.iamToken;
            }
            return null;
        }
        /// <summary>
        /// Отправляет запрос в GPT и возвращает ответ. Не забудьте прикрепить IamToken
        /// </summary>
        /// <param name="iamToken">Токен для верификации запроса. Получаем из функции YandexAuth</param>
        /// <param name="jsonRequest">Класс для запроса, является обязательным параметром. Вы можете отправить запрос</param>
        /// <returns>Возвращает класс YandexGptResponse</returns>
        public static YandexGPTresponse SendMessage(string iamToken, YandexJson jsonRequest)
        {

            string requestBody = JsonSerializer.Serialize(jsonRequest);

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {iamToken}");
            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("https://llm.api.cloud.yandex.net/foundationModels/v1/completion", content).Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<YandexGPTresponse>(responseContent);
        }

    }

    /// <summary>
    /// Класс который сереализуется в json для запроса к GPT
    /// </summary>
    public class YandexJson
    {
        public string modelUri { get; set; }
        public Completionoptions completionOptions { get; set; }
        public YandMessage[] messages { get; set; }
    }

    public class Completionoptions
    {
        public bool stream { get; set; }
        public float temperature { get; set; }
        public string maxTokens { get; set; }
    }

    public class YandMessage
    {
        public string role { get; set; }
        public string text { get; set; }
    }




    /// <summary>
    /// Является ответом после отправки зарпоса.
    /// </summary>
    public class YandexGPTresponse
    {
        public Result result { get; set; }
    }

    public class Result
    {
        public Alternative[] alternatives { get; set; }
        public Usage usage { get; set; }
        public string modelVersion { get; set; }
    }

    public class Usage
    {
        public string inputTextTokens { get; set; }
        public string completionTokens { get; set; }
        public string totalTokens { get; set; }
    }

    public class Alternative
    {
        public YandMessage message { get; set; }
        public string status { get; set; }
    }









    public class YandeAuthResponse
    {
        public string iamToken { get; set; }
        public string expiresAt { get; set; }
    }
}
