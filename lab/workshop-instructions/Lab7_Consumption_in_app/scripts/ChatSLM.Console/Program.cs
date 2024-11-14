// See https://aka.ms/new-console-template for more information


using ChatSLM.Utils;

// See https://aka.ms/new-console-template for more information


void ShowMenu()
{
    Console.WriteLine("Please select an option:");
    Console.WriteLine("1. Chat With Phi-3.5 & Phi-3.5-ft & GPT-3.5-ft with Travel Data");
    Console.WriteLine("2. Evaluation Phi-3.5-ft ONNX Model");
    Console.WriteLine("3. Exit");
}

void ChatWithModels()
{
    Console.WriteLine("Welcoment to ChatML Console! I am your ML assistant. Please ask me any question about travel. If you want to exit, please type ctrl + c.");

    while (true)
    {
        Console.Write("Ask: ");
        String? question = Console.ReadLine();

        if (String.IsNullOrWhiteSpace(question))
        {
            Console.WriteLine("Bot : Please input a question.");
            return;
        }


        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(Phi35): ");
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(1,question);

        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(Phi35-ft): " );
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(2,question,true);

        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(GPT35-ft): " );
        Console.ResetColor();

        Console.WriteLine(GenAI.ChatWithAOAI(question));

        Console.WriteLine("************************************************************************************************************" );


    }
}

void EvaluateModel()
{
    Console.WriteLine("Welcoment to ChatML Console! You can compare different models . If you want to exit, please type ctrl + c.");

    while (true)
    {
        Console.Write("Ask: ");
        String? question = Console.ReadLine();

        if (String.IsNullOrWhiteSpace(question))
        {
            Console.WriteLine("Bot : Please input a question.");
            return;
        }


        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(Phi35-ft): ");
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(2,question,true);

        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(optimization-Phi35-ft): " );
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(3,question,true);

        Console.WriteLine("************************************************************************************************************" );


    }
    // Add your evaluation logic here
}

while (true)
{
    ShowMenu();
    Console.Write("Select an option: ");
    String? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            ChatWithModels();
            break;
        case "2":
            EvaluateModel();
            break;
        case "3":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

// Console.WriteLine("Welcoment to ChatML Console! I am your ML assistant. Please ask me any question about ML and AI. If you want to exit, please type ctrl + c.");

// while(true){

//     Console.Write("Ask: ");
//     String? question = Console.ReadLine();

//     if (String.IsNullOrWhiteSpace(question))
//     {
//         Console.WriteLine("Bot : Please input a question.");
//         return;
//     }

//     Console.WriteLine("************************************************************************************************************" );

//     Console.Write("Bot(Phi35): " );

//     GenAI.ChatWithGenAIONNXOpenAIAsync(question);

//     Console.WriteLine("************************************************************************************************************" );


//     Console.Write("Bot(Phi35-ft): " );

//     GenAI.ChatWithGenAIONNXOpenAIAsync(question,true);

//     Console.WriteLine("************************************************************************************************************" );

// }