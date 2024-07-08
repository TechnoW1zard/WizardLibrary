using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WizardLibrary.API.GPTs.CozeApi
{
    public class CozeApi
    {
        /// <summary>
        /// Асинхронно отправляет сообщение с включенным стримингом через API Coze и возвращает StreamReader для потока ответа.
        /// </summary>
        /// <param name="bearerToken">Токен для аутентификации в API Coze.</param>
        /// <param name="payload">Полезная нагрузка, содержащая запрос и другие детали чата. Сообщение которое хотите добавить тоже нужно указать в payload</param>
        /// <param name="client">HttpClient, используемый для отправки запроса.</param>
        /// <returns>StreamReader, присоединенный к потоку ответа, позволяющий читать ответ в реальном времени. Могут вернуться json в виде ошибки, поэтому используйте класс EventErr для того чтобы распарсить ошибку. </returns>
        public static async Task<StreamReader> StreamMessage( string bearerToken, PayLoad payload, HttpClient client)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            payload.stream = true;
            var content = new StringContent(JsonConvert.SerializeObject(payload, settings), System.Text.Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
            client.DefaultRequestHeaders.Add("Connection", $"keep-alive");
            client.DefaultRequestHeaders.Add("Accept", $"*/*");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.coze.com/open_api/v2/chat") { Content = content };
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return new StreamReader(stream);
        }

    }

    /// <summary>
    /// Класс для получения ошибки
    /// </summary>
    public class EventErr : EventData
    {
        public Error_Information error_information { get; set; }
    }

    public class Error_Information
    {
        public int err_code { get; set; }
        public string err_msg { get; set; }
    }


    /// <summary>
    /// Как выглядит истоирия сообщений.
    /// </summary>
    public class HistoryMessage
    {
        public string role { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string content_type { get; set; }
    }

    /// <summary>
    /// Параметры для отправки запроса
    /// </summary>
    public class PayLoad
    {
        public string conversation_id { get; set; }
        public string bot_id { get; set; }
        public string user { get; set; }
        public string query { get; set; }
        public bool stream { get; set; }
        public List<HistoryMessage> chat_history { get; set; }
    }

    public class EventData
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("message")]
        public MessageData Message { get; set; }
        public bool is_finish { get; set; }
        public int index { get; set; }
        public string conversation_id { get; set; }
    }

    public class MessageData
    {
        public string role { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string content_type { get; set; }
        public string? extra_info { get; set; }
    }
}
