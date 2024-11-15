# Lab 2. Data Preperation
## Introduction

> [!NOTE]
>This is a **15-minute** workshop that will give you a hands-on introduction to the core concepts and best practices for data preparation.

## Learning Objectives

By the end of this workshop, you should be able to: 
1. Deploy a GPT-4 Model and interact with it in the Chat Playground
2. Use GPT-3.5 To create synthetic data based on a known scenario.
1. Prepare a dataset for fine-tuning with Azure AI Studio.
1. Verify that the datasets.
1. Understand the importance of the dataset preparation process.

## Lab Scenario
This lab will guide you through the process of creating data set for use in fine tuning experiementation. In this lab, you will prepare the dataset for fine-tuning. You will create and deploy a GPT model to create a synthetic data. In option2 you will download a known dataset using python to your local environment.

## Lab Outline
This lab comprises the following exercises:

- Create a GPT-4 OpenAI Deployment
- Option1. Create synthetic data using GPT-4 
    - Create a prompt to create synthetic data and save this as in jsonl format.
- Option 2. Prepare Dataset for Fine-tuning from a known Data Set. 

## Deploy a model

Azure provides a web-based portal named **Azure AI**, that you can use to deploy, manage, and explore models. You'll start your exploration of Azure OpenAI by using Azure AI Hub and Projects to deploy a model.

> **Note**: As you use Azure AI, message boxes suggesting tasks for you to perform may be displayed. You can close these and follow the steps in this exercise.

1. Within your current Azure AI project, in the left hand pane, select the **Deployments**. Create a new deployment of the **gpt-4** model with the following settings:
    - Select **+Deploy Model**
    - Select **Deploy base model**
    - Select **Gpt-4** and select confirm
    - **Deployment name**: *A unique name of your choice*
    - **Model version**: *Use default version*
    - **Deployment type**: Global Standard
    - **Tokens per minute rate limit**: 10K\*
    - **Content filter**: DefaultV2
    - Select **Deploy**

    > \* A rate limit of 10,000 tokens per minute is more than adequate to complete this exercise while leaving capacity for other people using the same subscription.

## Use the Chat playground

Now that you've deployed a model, you can use it to generate responses based on natural language prompts. The *Chat* playground in Azure AI provides a playground chatbot interface.

1. Select **Open in playground**
1. In the **Playground** section, select the **Chat** page. The **Chat** playground page consists of a row of buttons and two main panels (which may be arranged right-to-left horizontally, or top-to-bottom vertically depending on your screen resolution):
    - Left hand side Menu Option **Deployment** and **Give the Model Instruction and Context** - used to select your deployment, define system message, and set parameters for interacting with your deployment.
    - **Chat session** - used to submit chat messages and view responses.
1. Under **Deployments**, ensure that your gpt-4 model deployment is selected.
1. Create a default **System message**, which should be ```You are an AI assistant that helps people find information.``` The system message is included in prompts submitted to the model, and provides context for the model's responses; setting expectations about how an AI agent based on the model should interact with the user.
1. Select **Apply Changes** to save the system message
1. In the **Chat session** panel, enter the user query `How can I use generative AI to help me market a new product?`

    > **Note**: You may receive a response that the API deployment is not yet ready. If so, wait for a few minutes and try again.

1. Review the response, noting that the model has generated a cohesive natural language answer that is relevant to the query with which it was prompted.
1. Enter the user query `What skills do I need if I want to develop a solution to accomplish this?`.
1. Review the response, noting that the chat session has retained the conversational context (so "this" is interpreted as a generative AI solution for marketing). This contextualization is achieved by including the recent conversation history in each successive prompt submission, so the prompt sent to the model for the second query included the original query and response as well as the new user input.
1. In the **Chat session** panel toolbar, select **Clear chat** and confirm that you want to restart the chat session.
1. Enter the query `Can you help me find resources to learn those skills?` and review the response, which should be a valid natural language answer, but since the previous chat history has been lost, the answer is likely to be about finding generic skilling resources rather than being related to the specific skills needed to build a generative AI marketing solution.

## Prepare Dataset for Fine-tuning

### Using GPT to create synthetic Data 

Based on LLM context you need to provide prompt templates based on the requirements here are some examples:

In the **Playground** select your **GPT-3.5-turbo** in the **Setup** menu select **Parameters** set **max repsonse** and **2000**

Prompt For FineTuning GPT-3.5 Model

```
Generate me 10 sample travel agent interactions which could happen in the commerical travel industry with real world content of a maximum of 80 words be accurate and realistic. Extract conversations from each discussion in JSON Format with a Role:<string>. Content:<String> here are some examples: {"messages": [{"role": "system", "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."}, {"role": "user", "content": "What's a must-see in Paris?"}, {"role": "assistant", "content": "Oh la la! You simply must twirl around the Eiffel Tower and snap a chic selfie! After that, consider visiting the Louvre Museum to see the Mona Lisa and other masterpieces. What type of attractions are you most interested in?"}]}
{"messages": [{"role": "system", "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."}, {"role": "user", "content": "Can you recommend a beach destination that's not too crowded?"}, {"role": "assistant", "content": "Absolutely! Have you heard of the hidden gem, Cala Gonone in Sardinia? It's like having your own slice of paradise without the crowds. You can enjoy crystal-clear waters, explore nearby caves, and even go hiking in the surrounding mountains. Is that the kind of destination you're looking for?"}]}
{"messages": [{"role": "system", "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."}, {"role": "user", "content": "I love history, where should I go on my next trip?"}, {"role": "assistant", "content": "History buff, eh? Time travel to Rome and walk in the footsteps of gladiators at the Colosseum! You can also explore the Roman Forum and the Pantheon. Or would you prefer to unravel the mysteries of the pyramids in Egypt and visit the ancient temples of Luxor? Do you have a preference for a specific region?"}]}
```

The final output should be in this format.

```
{"messages": [{"role": "user", "content": "What are some must-try foods in Thailand?"}, {"role": "assistant", "content": "Thailand is a food lover's paradise! Some must-try dishes include pad Thai, green curry, tom yum soup, mango sticky rice, and som tam (spicy papaya salad). Don't forget to try street food like grilled satay skewers and crispy fried spring rolls. Each region in Thailand has its own unique specialties, so you'll have plenty of delicious options to choose from. Are you interested in any specific region in Thailand?"}]}
{"messages": [{"role": "user", "content": "What are the visa requirements for traveling to Australia?"}, {"role": "assistant", "content": "To travel to Australia, most visitors need a visa. The type of visa you'll need depends on your nationality and the purpose of your visit. The most common visa for tourists is the Electronic Travel Authority (ETA) or eVisitor visa, which allows you to stay in Australia for up to 3 months. It's important to check the specific visa requirements based on your nationality before planning your trip."}]}
{"messages": [{"role": "user", "content": "I want to go on a road trip in the USA, any suggestions?"}, {"role": "assistant", "content": "The USA offers endless possibilities for road trips! One iconic route is the Pacific Coast Highway, which stretches along the California coastline from San Francisco to Los Angeles, offering stunning ocean views. Another popular option is Route 66, which spans from Chicago to Los Angeles and takes you through diverse landscapes and charming towns. Do you have a preference for a specific region in the USA?"}]}
{"messages": [{"role": "user", "content": "What are some popular outdoor activities in New Zealand?"}, {"role": "assistant", "content": "New Zealand is a playground for outdoor enthusiasts! Some popular activities include hiking the Milford Track in Fiordland National Park, bungee jumping in Queenstown, kayaking in Abel Tasman National Park, and exploring the geothermal wonders of Rotorua. You can also go skiing in the winter or take a scenic helicopter ride over the stunning landscapes. Is there anything specific you'd like to do in New Zealand?"}]}
{"messages": [{"role": "user", "content": "I want to go scuba diving in Southeast Asia, any recommendations?"}, {"role": "assistant", "content": "Southeast Asia is a diver's paradise! Some top destinations for scuba diving include the Similan Islands in Thailand, the Tubbataha Reefs Natural Park in the Philippines, and the Komodo National Park in Indonesia. You can explore vibrant coral reefs, encounter colorful marine life, and even swim with majestic manta rays and whale sharks. Do you have a preference for a specific country in Southeast Asia?"}]}
{"messages": [{"role": "user", "content": "What's the best way to get around in Amsterdam?"}, {"role": "assistant", "content": "Amsterdam has an excellent public transportation system, including trams, buses, and metros, which can take you to most major attractions and neighborhoods. You can also rent a bike and explore the city like a local, as Amsterdam is known for its bike-friendly infrastructure. Taxis and ride-sharing services are also available, but they can be more expensive. It's a city that's easy to navigate, so you'll have no trouble getting around."}]}
{"messages": [{"role": "user", "content": "What are some popular festivals in India?"}, {"role": "assistant", "content": "India is known for its vibrant festivals! Some popular ones include Diwali, the Festival of Lights, which celebrates the victory of light over darkness and is celebrated with fireworks, sweets, and decorations. Holi, the Festival of Colors, is another major celebration where people throw colored powders and water at each other. Navratri, Ganesh Chaturthi, and Durga Puja are also widely celebrated across different regions in India. Do you have a preference for a specific festival or region?"}]}
{"messages": [{"role": "user", "content": "What are some must-see attractions in New York City?"}, {"role": "assistant", "content": "New York City is filled with iconic attractions! Some must-sees include Times Square, Central Park, the Statue of Liberty, the Empire State Building, and the Metropolitan Museum of Art. You can also explore neighborhoods like Chinatown, Greenwich Village, and Brooklyn. Don't forget to try some famous New York-style pizza and bagels along the way. Is there anything specific you'd like to do in New York City?"}]}
{"messages": [{"role": "user", "content": "What's the best time to visit the Great Barrier Reef?"}, {"role": "assistant", "content": "The best time to visit the Great Barrier Reef is during the dry season, which is from June to October. During this time, the weather is generally sunny and the water is clear, making it ideal for snorkeling and diving. However, the Great Barrier Reef is a year-round destination, and even during the wet season, you can still have a great time exploring the reef and its marine life. Is there anything specific you'd like to do at the Great Barrier Reef?"}]}
{"messages": [{"role": "user", "content": "What are some popular attractions in London?"}, {"role": "assistant", "content": "London has a wealth of attractions! Some popular ones include the British Museum, Buckingham Palace, the Tower of London, the London Eye, and the West End theater district. You can also explore iconic landmarks like Big Ben, Westminster Abbey, and the Houses of Parliament. Don't forget to indulge in some fish and chips and afternoon tea while you're there. Is there anything specific you'd like to do in London?"}]}
```

Simply copy and paste the output json into a notepad and save the file as `data_sample.jsonl` .jsonl` is the format required for fine tuning experiementation

Prompt for Phi-3 mini instruct training 

```
Generate me 20 sample travel agent interactions which could happen in the commerical travel industry with real world content of a maximum of 80 words be accurate and realistic. Extract conversations from each discussion in JSON Format with a Prompt:<string>. Response:<String> here are some examples {"prompt": "What are the top attractions in Rome?", "response": "Rome, the Eternal City! You can't miss the Colosseum, the Vatican City, and the Pantheon. Don't forget to explore the charming streets of Trastevere and indulge in delicious Italian cuisine. Want to visit any specific historical site or try a local dish?"} {"prompt": "Can you recommend a family-friendly resort in the Maldives?", "response": "The Maldives is a tropical paradise! I'd suggest the Soneva Fushi resort, which offers luxurious villas and a kids' club. Alternatively, the Cheval Blanc Randheli resort has a water sports center and a kids' program. Both are perfect for a family vacation. Are you looking for snorkeling or diving activities?"} 
```

The final output should be in this format.

```
[ {"prompt": "What are the best beaches in Thailand?", "response": "Thailand offers stunning beaches! Phuket's Patong Beach and Krabi's Railay Beach are top choices. For a quieter spot, try Koh Lanta. Are you interested in beach activities like snorkeling or just relaxing?"}, {"prompt": "Can you suggest a romantic getaway in Italy?", "response": "For a romantic getaway, consider Venice with its canals and gondola rides. Alternatively, Tuscany offers beautiful countryside and wineries. Are you looking for city romance or a countryside retreat?"}, {"prompt": "What's the best time to visit Japan for cherry blossoms?", "response": "Cherry blossom season in Japan is typically from late March to early April. Tokyo and Kyoto are great places to enjoy the blossoms. Would you like help planning your visit?"}, {"prompt": "Which Caribbean islands are best for scuba diving?", "response": "The Caribbean has excellent scuba diving spots! Consider Grand Cayman for its clear waters, or Bonaire for its marine parks. Are you a beginner or an experienced diver?"}, {"prompt": "What are the must-see sights in Paris?", "response": "Paris has many must-see sights like the Eiffel Tower, Louvre Museum, and Notre-Dame Cathedral. Don't miss a Seine River cruise. Any specific landmarks or museums you're interested in?"}, {"prompt": "Can you recommend a budget-friendly European city break?", "response": "Budapest is great for a budget-friendly city break with its thermal baths and historic sites. Porto in Portugal is also a wonderful and affordable choice. Looking for culture, food, or both?"}, {"prompt": "What is there to do in Sydney?", "response": "Sydney offers the Sydney Opera House, Harbour Bridge, and Bondi Beach. Don't miss the Taronga Zoo and a walk through the Royal Botanic Garden. Interested in any specific activities or sites?"}, {"prompt": "Can you suggest a luxurious resort in Bali?", "response": "For luxury in Bali, consider the Four Seasons at Sayan or the St. Regis Bali Resort. Both offer exceptional amenities and stunning views. Are you interested in spa treatments or beachside relaxation?"}, {"prompt": "What are the best places to visit in New Zealand?", "response": "New Zealand highlights include Milford Sound, Queenstown, and Rotorua. For a city experience, visit Auckland or Wellington. Interested in adventure activities or scenic drives?"}, {"prompt": "What travel documents do I need for a trip to Canada?", "response": "You'll need a valid passport and possibly a visa or eTA, depending on your nationality. Ensure your passport has at least six months' validity. Need help with visa requirements?"}, {"prompt": "Which cities in Europe are best for a Christmas market tour?", "response": "European cities like Vienna, Prague, and Strasbourg are famous for their Christmas markets. Each offers unique festive experiences. Are you planning a trip in December?"}, {"prompt": "Can you recommend a wildlife safari in Africa?", "response": "Consider a safari in Kenya's Maasai Mara or South Africa's Kruger National Park. Both offer incredible wildlife viewing. Are you looking for a luxury lodge or a more rustic experience?"}, {"prompt": "What are the top attractions in New York City?", "response": "In NYC, don't miss Times Square, Central Park, and the Statue of Liberty. The Metropolitan Museum of Art is also a must-see. Any particular neighborhoods or attractions you're interested in?"}, {"prompt": "Which Asian destinations are best for a cultural trip?", "response": "Kyoto in Japan and Siem Reap in Cambodia are great for cultural trips. Both offer rich history and stunning temples. Looking for guided tours or self-exploration?"}, {"prompt": "What activities are available on a cruise to Alaska?", "response": "Alaska cruises offer glacier viewing, whale watching, and excursions to historic towns like Skagway. You can also enjoy onboard amenities like spas and restaurants. Interested in a particular cruise line?"}, {"prompt": "Can you suggest a unique experience in Iceland?", "response": "In Iceland, try snorkeling between tectonic plates at Silfra, or visiting the Blue Lagoon for geothermal bathing. Seeing the Northern Lights is also unforgettable. Any preferences for your adventure?"}, {"prompt": "What are the best national parks to visit in the USA?", "response": "Yosemite, Grand Canyon, and Yellowstone are top national parks in the USA. Each offers unique landscapes and activities. Are you interested in hiking, wildlife, or scenic drives?"}, {"prompt": "Can you recommend a honeymoon destination in the Caribbean?", "response": "Consider St. Lucia with its luxurious resorts and scenic Pitons, or the Bahamas for stunning beaches and crystal-clear waters. Looking for all-inclusive options or more private getaways?"}, {"prompt": "What are the top culinary destinations in Europe?", "response": "For culinary experiences, visit Paris for French cuisine, Bologna in Italy for pasta, and Barcelona for tapas. Each city offers unique flavors. Are you looking for cooking classes or food tours?"}, {"prompt": "Which destinations are best for a solo female traveler?", "response": "Japan, especially Kyoto and Tokyo, and New Zealand are great for solo female travelers. Both are safe and offer plenty to see and do. Interested in guided tours or self-guided adventures?"} ]
```
Copy and paste the output into a notepad and save as `phidatasample.jsonl`

To undertake fine tuning we recommed you create 60 samples. To get 60 samples you may need to do a few back an forth with the model and ensure **Max response token to 4096 and specify you need 60 sample**.   


