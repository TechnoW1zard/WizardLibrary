
using System.Text;
using System.Text.Json;

namespace WizardLibrary.API.GPTs.Gpt4All
{
    public class Gpt4AllApi
    {

        public static Gpt4AllResponse SendMessage(Gpt4AllRequest jsonRequest, int port)
        {

            string requestBody = JsonSerializer.Serialize(jsonRequest);
            using HttpClient client = new HttpClient();
            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync($"http://localhost:{port}/v1/chat/completions", content).Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<Gpt4AllResponse>(responseContent);
        }
    }

    /// <summary>
    /// Класс для создания запроса к Gpt4All
    /// </summary>
    public class Gpt4AllRequest
    {
        public string model { get; set; }
        public Message[] messages { get; set; }
        public float temperature { get; set; }
        public int max_tokens { get; set; }
        public bool stream { get; set; }
    }


    /// <summary>
    /// Ответ от gpt4all
    /// </summary>
    public class Gpt4AllResponse
    {
        public Choice[] choices { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string _object { get; set; }
        public Usage usage { get; set; }
    }

    public class Usage
    {
        public int completion_tokens { get; set; }
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Choice
    {
        public string finish_reason { get; set; }
        public int index { get; set; }
        public Message message { get; set; }
        public object[] references { get; set; }
    }

    public class Message
    {
        public string content { get; set; }
        public string role { get; set; }
    }

}
