using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatWithSLM.UI.Utils;

public class SLMClient(HttpClient httpClient)
{
    public async Task<string> GeChatCompletionAsync(ChatRequest content)
    {
        var response = await httpClient.PostAsJsonAsync("/v1/chat/completions", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}



public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class Choice
{

    [JsonPropertyName("index")]
    public string Index { get; set; }
    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

public class ChatCompletion
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]

    public string Object { get; set; }

    [JsonPropertyName("created")]

    public string Created { get; set; }


    [JsonPropertyName("model")]
    public string Model { get; set; }


    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
}

public class ChatRequest
{
    public List<Message> Messages { get; set; }
    public string Model { get; set; }
}