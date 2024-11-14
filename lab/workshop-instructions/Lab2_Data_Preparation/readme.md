# Lab 2. Data Preperation
## Introduction

> [!NOTE]
>This is a **15-minute** workshop that will give you a hands-on introduction to the core concepts and best practices for data preparation.

## Learning Objectives

By the end of this workshop, you should be able to: 
1. Use GPT To create synthetic data based on a known scenario.
1. Prepare a dataset for fine-tuning with Azure AI Studio.
1. Download the dataset using the a python script file.
1. Verify that the datasets were saved successfully.
1. Understand the importance of the dataset preparation process.

## Lab Scenario
This lab will guide you through the process of creating data set for use in fine tuning experiementation. In this lab, you will prepare the dataset for fine-tuning. You will create and deploy a GPT model to create a synthetic data. In option2 you will download a known dataset using python to your local environment.

## Lab Outline
This lab comprises the following exercises:

- Create an OpenAI Deployment
- Option1. Create synthetic data using GPT 
    - Create a prompt to create synthetic data and save this as in jsonl format.
- Option 2. Prepare Dataset for Fine-tuning from a known Data Set. 

## Deploy a model

Azure provides a web-based portal named **Azure AI**, that you can use to deploy, manage, and explore models. You'll start your exploration of Azure OpenAI by using Azure AI Hub and Projects to deploy a model.

> **Note**: As you use Azure AI, message boxes suggesting tasks for you to perform may be displayed. You can close these and follow the steps in this exercise.

1. In Azure AI, in the pane on the left, select the **Deployments**. If you don't already have one, create a new deployment of the **gpt-4** model with the following settings:
    - Select **+Deploy Model**
    - Select **Deploy base model**
    - Select **Gpt-4** and selet confirm
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
1. Create a default **System message**, which should be *You are an AI assistant that helps people find information.* The system message is included in prompts submitted to the model, and provides context for the model's responses; setting expectations about how an AI agent based on the model should interact with the user.
1. Select **Save**
1. In the **Chat session** panel, enter the user query `How can I use generative AI to help me market a new product?`

    > **Note**: You may receive a response that the API deployment is not yet ready. If so, wait for a few minutes and try again.

1. Review the response, noting that the model has generated a cohesive natural language answer that is relevant to the query with which it was prompted.
1. Enter the user query `What skills do I need if I want to develop a solution to accomplish this?`.
1. Review the response, noting that the chat session has retained the conversational context (so "this" is interpreted as a generative AI solution for marketing). This contextualization is achieved by including the recent conversation history in each successive prompt submission, so the prompt sent to the model for the second query included the original query and response as well as the new user input.
1. In the **Chat session** panel toolbar, select **Clear chat** and confirm that you want to restart the chat session.
1. Enter the query `Can you help me find resources to learn those skills?` and review the response, which should be a valid natural language answer, but since the previous chat history has been lost, the answer is likely to be about finding generic skilling resources rather than being related to the specific skills needed to build a generative AI marketing solution.

### Prepare Dataset for Fine-tuning

> [!NOTE]
> For the Ignite workshop please focus on Option1 only due to time constraints. 

## Option1: Using GPT to create synthetic Data 


Based on LLM context you need to provide prompt templates based on the requirements here are some examples:

** Prompt:**
Generate me some sample travel agent interactions which could happen in the commerical travel industry with realise content of a maximum of 80 words be accurate and realistic. Extact conversations from each discussion in JSON Format with a Prompt:<string>. Response:<String> here are some examples {"prompt": "What are the top attractions in Rome?", "response": "Rome, the Eternal City! You can't miss the Colosseum, the Vatican City, and the Pantheon. Don't forget to explore the charming streets of Trastevere and indulge in delicious Italian cuisine. Want to visit any specific historical site or try a local dish?"}, {"prompt": "Can you recommend a family-friendly resort in the Maldives?", "response": "The Maldives is a tropical paradise! I'd suggest the Soneva Fushi resort, which offers luxurious villas and a kids' club. Alternatively, the Cheval Blanc Randheli resort has a water sports center and a kids' program. Both are perfect for a family vacation. Are you looking for snorkeling or diving activities?"} The final output should be in this format.

```
{  
  "messages": [  
    {  
      "role": "system",  
      "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."  
    },  
    {  
      "role": "user",  
      "content": "What should I wear when visiting temples in Thailand?"  
    },  
    {  
      "role": "assistant",  
      "content": "When visiting temples in Thailand, it's important to dress modestly. Wear clothing that covers your shoulders and knees, such as long pants and shirts with sleeves. Would you like to know about the cultural significance of any specific temples?"  
    }  
  ]  
},  
{  
  "messages": [  
    {  
      "role": "system",  
      "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."  
    },  
    {  
      "role": "user",  
      "content": "What's the weather like in Cape Town during December?"  
    },  
    {  
      "role": "assistant",  
      "content": "December is a great time to visit Cape Town as it's the beginning of summer there. Expect warm, sunny days with little rain. It's perfect for visiting the beaches or taking a hike up Table Mountain. Are you interested in outdoor activities or more relaxed sightseeing?"  
    }  
  ]  
},  
{  
  "messages": [  
    {  
      "role": "system",  
      "content": "You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms. You should not provide any hotel, flight, rental car or restaurant recommendations. Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday."  
    },  
    {  
      "role": "user",  
      "content": "Do I need a visa for a short trip to Japan?"  
    },  
    {  
      "role": "assistant",  
      "content": "Visa requirements for Japan depend on your nationality. Many countries benefit from visa exemption for stays up to 90 days. Could you tell me your passport country so I can provide more precise information?"  
    }  
  ]  
}  
```

Tou undertake fine tuning we recommed you create 60 samples Simply copy and paste into a notepad and save the file as `data_sample.jsonl` format for using within your fine tuning experiementation.


## Option2: Using A DataSet to Create Training Data
In this exercise, you will run the download_dataset.py file to download the HuggingFace Travel Planner Dataset +++https://huggingface.co/datasets/osunlp/TravelPlanner+++ to your local environment. You will then use this datasets to fine-tune the model using a DataSet.

In this exercise, you will:

Add code to the 'download_dataset.py' file to download the datasets.
Run the 'download_dataset.py' file to download datasets to your local environment.
 
### Download your dataset using download_dataset.py

- Create a new Python file  `download_dataset.py` file in Visual Studio Code.

- Add the following code into `download_dataset.py`.

```Python
import json
import os
from datasets import load_dataset
from config import (
    TRAIN_DATA_PATH,
    TEST_DATA_PATH)

def load_and_split_dataset(dataset_name, config_name, split_ratio):
    """
    Load and split a dataset.
    """
    # Load the dataset with the specified name, configuration, and split ratio
    dataset = load_dataset(dataset_name, config_name, split=split_ratio)
    print(f"Original dataset size: {len(dataset)}")
    
    # Split the dataset into train and test sets (80% train, 20% test)
    split_dataset = dataset.train_test_split(test_size=0.2)
    print(f"Train dataset size: {len(split_dataset['train'])}")
    print(f"Test dataset size: {len(split_dataset['test'])}")
    
    return split_dataset

def save_dataset_to_jsonl(dataset, filepath):
    """
    Save a dataset to a JSONL file.
    """
    # Create the directory if it does not exist
    os.makedirs(os.path.dirname(filepath), exist_ok=True)
    
    # Open the file in write mode
    with open(filepath, 'w', encoding='utf-8') as f:
        # Iterate over each record in the dataset
        for record in dataset:
            # Dump the record as a JSON object and write it to the file
            json.dump(record, f)
            # Write a newline character to separate records
            f.write('\n')
    
    print(f"Dataset saved to {filepath}")

def main():
    """
    Main function to load, split, and save the dataset.
    """
    # Load and split the travelplanner dataset with a specific configuration and split ratio
    dataset = load_and_split_dataset("osunlp/TravelPlanner", 'default', 'train_sft[:1%]')
    
    # Extract the train and test datasets from the split
    train_dataset = dataset['train']
    test_dataset = dataset['test']

    # Save the train dataset to a JSONL file
    save_dataset_to_jsonl(train_dataset, TRAIN_DATA_PATH)
    
    # Save the test dataset to a separate JSONL file
    save_dataset_to_jsonl(test_dataset, TEST_DATA_PATH)

if __name__ == "__main__":
    main()
``` 

> [!TIP]
>Guidance for fine-tuning with a minimal dataset using a CPU If you want to use a CPU for fine-tuning, this approach is ideal for those with benefit subscriptions (such as Visual Studio Enterprise Subscription) or to quickly test the fine-tuning and deployment process. Replace `dataset = load_and_split_dataset("osunlp/TravelPlanner", 'default', 'train_sft[:1%]')` with `dataset = load_and_split_dataset("osunlp/TravelPlanner", 'default', 'train_sft[:10]')`

 
- Type the following command inside your terminal to run the script and download the dataset to your local environment.

```Python
python download_dataset.py
```

- Verify that the datasets were saved successfully to your local finetune/data directory.


> [!NOTE]
>Note on dataset size and fine-tuning time. In this tutorial, you use only 1% of the dataset (train_sft[:1%]). This significantly reduces the amount of data, speeding up both the upload and fine-tuning processes. You can adjust the percentage to find the right balance between training time and model performance. Using a smaller subset of the dataset reduces the time required for fine-tuning, making the process more manageable for a tutorial.


## Option3: Using Gretel.ai

To create synthetic data using Gretel.ai +++https://gretel.ai+++, follow these steps:

- Create a Project: Log into the Gretel console +++https://gretel.ai+++ and create a new project.

- Upload Data: Upload a CSV file or DataFrame containing the data you want to synthesize.

- Generate Synthetic Data: Choose to generate synthetic data and customize the configuration if needed.

- Train the Model: Wait for the model to train and generate synthetic records.

- Review Quality: Review the synthetic data quality report to compare statistics and distributions with the original data.

- Download Data: Download the synthetic dataset and model for further use or sharing.
