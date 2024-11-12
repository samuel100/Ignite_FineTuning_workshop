using System.ClientModel;
using Azure;  
using Azure.AI.OpenAI;  
using Azure.Identity;  
using OpenAI.Chat; 

namespace ChatWithSLM.UI.Utils;


public class AOAIClient()
{
    public static string ChatWithAOAI(string prompt){

        string aoai_endpoint = "Your AOAI Service Endpoint";
        string aoai_key = "Your AOAI Service Key";
        string aoai_model = "Your GPT-3.5 Fine tuning Deployment name";


        AzureOpenAIClient azureClient = new(
            new Uri(aoai_endpoint),
            new ApiKeyCredential(aoai_key));

        ChatClient chatClient = azureClient.GetChatClient(aoai_model);

        var completion = chatClient.CompleteChat(
        [
            new UserChatMessage(prompt),
        ]);

        return completion.Value.Content[0].Text;
    }


}