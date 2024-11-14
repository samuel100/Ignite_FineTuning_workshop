namespace ChatSLM.Utils;

using System;
using System.Reflection;
using System.Threading.Tasks;

using System.ClientModel;
using Azure;  
using Azure.AI.OpenAI;  
using Azure.Identity;  
using OpenAI.Chat; 

using Microsoft.ML.OnnxRuntimeGenAI;

public class GenAI
{

    private static string modelPath = @"/home/azureuser/localfiles/Ignite_FineTuning_workshop/lab/workshop-instructions/Lab5-Optimize-Model/models/phi/onnx-ao/model";    
    private static string adapterPath = @"/home/azureuser/localfiles/Ignite_FineTuning_workshop/lab/workshop-instructions/Lab5-Optimize-Model/models/phi/onnx-ao/model/adapter_weights.onnx_adapter";

    private static string aoai_endpoint = "Your Azure OpenAI GPT Fine tuned GPT-3.5 endpoint";
    private static string aoai_key = "Your Azure OpenAI GPT-3.5 Fine tuned endpoint key";
    private static string aoai_model = "Your Azure OpenAI GPT-3.5 Fine tuned endpoint deploymen";

    private static Microsoft.ML.OnnxRuntimeGenAI.Model model = null;
    private static Microsoft.ML.OnnxRuntimeGenAI.Tokenizer  tokenizer = null;
    private static Microsoft.ML.OnnxRuntimeGenAI.Adapters  adapters = null;

   
    public static void InitGenAI()
    {
        model = new Model(modelPath);    
        tokenizer = new Tokenizer(model);
        adapters = new Adapters(model);
        adapters.LoadAdapter(adapterPath, "travel");
    }

    public static void ChatWithGenAIONNXOpenAIAsync(string prompt, bool set_adapter = false)
    {
        string userPrompt = prompt;
        string chatTemplate = "";

        chatTemplate = "<|user|>" + userPrompt + "\n<|end|>\n<|assistant|>";

        var tokens = tokenizer.Encode(chatTemplate);

        var generatorParams = new GeneratorParams(model);
        generatorParams.SetSearchOption("max_length", 200);
        generatorParams.SetInputSequences(tokens);
        generatorParams.SetSearchOption("past_present_share_buffer", false);

        var tokenizerStream = tokenizer.CreateStream();

        Generator generator = new(model, generatorParams);

        if(set_adapter)
        {
            generator.SetActiveAdapter(adapters, "travel");
        }

        while (!generator.IsDone())
        {
            generator.ComputeLogits();
            generator.GenerateNextToken();
            Console.Out.Write(tokenizerStream.Decode(generator.GetSequence(0)[^1]));
            Console.Out.Flush();
        }
        Console.WriteLine();

    }


    public static string ChatWithAOAI(string prompt){


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