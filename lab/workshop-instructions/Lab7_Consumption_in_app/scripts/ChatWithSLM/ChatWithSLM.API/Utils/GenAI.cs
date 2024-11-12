namespace ChatWithSLM.API.Utils;

using System;
using System.Threading.Tasks;

using Microsoft.ML.OnnxRuntimeGenAI;
using ChatWithSLM.API.Domains;


public class GenAI
{

    private static string modelPath = @"Your Phi-3.5 ONNX local Model Path";

    private static string ftmodelPath = @"Your Phi-3.5 Fine-tuned ONNX local Model Path";
    private static string ftadapterPath = @"Your Phi-3.5 Fine-tuned ONNX local Adapter Path";

    private static Microsoft.ML.OnnxRuntimeGenAI.Model model = null;
    private static Microsoft.ML.OnnxRuntimeGenAI.Tokenizer  tokenizer = null;


    private static Microsoft.ML.OnnxRuntimeGenAI.Adapters  adapters = null;

    private static int status = -1;

    private static int ft_status = -1;


   
    public static void InitGenAI(bool ft = false)
    {
        if(ft)
        {
            ft_status = 1;
        }
        else
        {
            ft_status = 0;
        }
        if(status != ft_status)
        {

            if(ft)
            {
                model = new Model(ftmodelPath);
                tokenizer = new Tokenizer(model);
                adapters = new Adapters(model);
                adapters.LoadAdapter(ftadapterPath, "travel");
            }
            else
            {
                model = new Model(modelPath);
                tokenizer = new Tokenizer(model);
            }

            status = ft_status;

            
        }

    }
    public static RepsonseJson ChatWithGenAIONNXOpenAIAsync(IList<ChatMessage> messages, bool ft = false)
    {
        InitGenAI(ft);

        string userPrompt = "";
        string systemPrompt =  "";
        string chatTemplate = "";

        foreach(var item in messages)
        {
            if(item.Role == "system")
            {
                systemPrompt = item.Content;
            }
            else
            {
                userPrompt = item.Content;
            }
        }


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

        string outputString="";



        while (!generator.IsDone())
        {

                generator.ComputeLogits();
                generator.GenerateNextToken();
                outputString += tokenizerStream.Decode(generator.GetSequence(0)[^1]);

        }

        return new RepsonseJson{ generated_text = outputString};
    }

}