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

    private static string modelPath = @"Phi-3.5 Mini Instruct ONNX Model Path";

    private static string oftmodelPath = @"Your Phi-3.5 Fine-tuned with travel data ONNX optimization Model Path";
    private static string oftadapterPath = @"Your Phi-3.5 Fine-tuned with travel data optimization adapter_weights.onnx_adapter path";


    private static string aoai_endpoint = "Your Azure OpenAI GPT Fine tuned GPT-3.5 endpoint";
    private static string aoai_key = "Your Azure OpenAI GPT-3.5 Fine tuned endpoint key";
    private static string aoai_model = "Your Azure OpenAI GPT-3.5 Fine tuned endpoint deploymen";

    private static Microsoft.ML.OnnxRuntimeGenAI.Model model = null;
    private static Microsoft.ML.OnnxRuntimeGenAI.Tokenizer  tokenizer = null;


    private static Microsoft.ML.OnnxRuntimeGenAI.Adapters  adapters = null;

    private static int status = -1;

    private static int ft_status = -1;


   
    public static void InitGenAI(int status, bool ft = false)
    {
        if (ft)
        {
            ft_status = 1;
        }
        else
        {
            ft_status = 0;
        }
        if (status != ft_status)
        {
            if (ft)
            {
                if (status == 2)
                {
                    model = new Model("path_to_new_model");
                }
                else
                {
                    model = new Model(oftmodelPath);
                }
                tokenizer = new Tokenizer(model);
                adapters = new Adapters(model);
                if (status == 2)
                {
                    adapters.LoadAdapter("path_to_new_adapter", "travel");
                }
                else
                {
                    adapters.LoadAdapter(oftadapterPath, "travel");
                }
            }
            else
            {
                // Continue with the rest of your method
            }
        }
    }

    public static void ChatWithGenAIONNXOpenAIAsync(int status,string prompt, bool ft = false)
    {
        InitGenAI(status,ft);

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

        if(ft)
        {
            generator.SetActiveAdapter(adapters, "travel");
        }



        int token_count = 0;
        while (!generator.IsDone())
        {
            generator.ComputeLogits();
            generator.GenerateNextToken();
            // var token = generator.GetSequence(0)[token_count];
            // Console.Out.Write(tokenizerStream.Decode(token));
            Console.Out.Write(tokenizerStream.Decode(generator.GetSequence(0)[^1]));
            Console.Out.Flush();
            token_count++;

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