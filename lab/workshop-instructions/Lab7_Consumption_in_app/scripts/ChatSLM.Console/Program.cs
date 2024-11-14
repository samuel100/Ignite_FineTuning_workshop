// See https://aka.ms/new-console-template for more information


using ChatSLM.Utils;

// See https://aka.ms/new-console-template for more information


void ShowMenu()
{
    Console.WriteLine("Please select an option:");
    Console.WriteLine("1. Chat With Phi-3.5 Base, Phi-3.5 with Adapter, & GPT-3.5-ft with Travel Data");
    Console.WriteLine("2. Exit");
}

void ChatWithModels()
{
    Console.WriteLine("Welcome to ChatML Console! I am your ML assistant. Please ask me any question about travel. If you want to exit, please type ctrl + c.");
    GenAI.InitGenAI();

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
        Console.Write("Bot(Phi35-base): ");
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(question, false);

        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(Phi35-with-ft-adapter): " );
        Console.ResetColor();

        GenAI.ChatWithGenAIONNXOpenAIAsync(question,true);

        Console.WriteLine("************************************************************************************************************" );

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot(GPT35-ft): " );
        Console.ResetColor();

        Console.WriteLine(GenAI.ChatWithAOAI(question));

        Console.WriteLine("************************************************************************************************************" );


    }
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
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
