#Lab 3. Fine-tune a language model for chat completion in the Azure AI Studio

## Introduction

In this exercise, you'll fine-tune a language model with the Azure AI Studio that you want to use for a custom copilot scenario.

This exercise will take approximately **45** minutes.

## Learning Objectives
By the end of this workshop, you should be able to:
1. Explore the model catalog in Azure AI Studio.
1. Deploy a model from the model catalog
1. Fine-tune and deploy the GPT model in Azure AI Studio
1. Test the fine-tuned model

## Lab Scenario
The lab scenario of this lab, you will fine-tune the GPT model using Azure AI Studio and Azure Model Catalog.

## Lab Outline
This lab consists of the following exercises:
1. Fine-tune and Deploy the GPT model in Azure AI Studio
1. The Azure AI Studio’s model catalog serves as a central repository where you can explore and use a variety of models, facilitating the creation of your generative AI scenario.

## Create an Azure AI hub

You need an [Azure AI Studio hub](https://ai.azure.com) in your Azure subscription to host projects. You can either create this resource while creating a project, 

1. Go to [Azure AI Studio](https://ai.azure.com)
2. In the **Management** section, select **All hubs**, then select **+ New hub**. Create a new hub with the following settings:
    - **Hub name**: *A unique name*
    - **Subscription**: *Your Azure subscription*
    - **Resource group**: *Create a new resource group with a unique name, or select an existing one*
    - **Location**: *Make a **random** choice from any of the following regions*\*
        - Australia East
        - Canada East
        - East US
        - East US 2
        - France Central
        - Japan East
        - North Central US
        - Sweden Central
        - Switzerland North
        - UK South
    - **Connect Azure AI Services or Azure OpenAI**: *Select to create a new AI Services or use an existing one*
    - **Connect Azure AI Search**: Skip connecting

> [!NOTE] 
> Azure OpenAI resources are constrained at the tenant level by regional quotas. The listed regions include default quota for the model type(s) used in this exercise. Randomly choosing a region reduces the risk of a single region reaching its quota limit in scenarios where you are sharing a tenant with other users. In the event of a quota limit being reached later in the exercise, there's a possibility you may need to create another resource in a different region.


After the Azure AI hub has been created, it should look similar to the following image:

![Screenshot of a Azure AI hub details in Azure AI Studio.](./images/azure-ai-resource.png)

1. Open a new browser tab (leaving the Azure AI Studio tab open) and browse to the Azure portal at [https://portal.azure.com](https://portal.azure.com?azure-portal=true), signing in with your Azure credentials if prompted.
1. Browse to the resource group where you created your Azure AI hub, and view the Azure resources that have been created.

    ![Screenshot of an Azure AI hub and related resources in the Azure portal.](./images/azure-portal.png)

1. Return to the Azure AI Studio browser tab.
1. View each of the pages in the pane on the left side of the page for your Azure AI hub, and note the artifacts you can create and manage. On the **Connections** page, observe that connections to Azure OpenAI and AI services have already been created.

## Create a project

An Azure AI hub provides a collaborative workspace within which you can define one or more *projects*. Let's create a project in your Azure AI hub.

1. In [Azure AI Studio](https://ai.azure.com), ensure you're in the hub you just created (you can verify your location by checking the path at the top of the screen).
1. Navigate to **All projects** using the menu on the left.
1. Select **+ New project**.
1. In the **Create a new project** wizard, create a project with the following settings:
    - **Current hub**: *Your AI hub*
    - **Project name**: *A unique name for your project*
1. Wait for your project to be created. When it's ready, it should look similar to the following image:

    ![Screenshot of a project details page in Azure AI Studio.](./images/azure-ai-project.png)

1. View the pages in the pane on the left side, expanding each section, and note the tasks you can perform and the resources you can manage in a project.

## Choose a model using model benchmarks

Before deploying a model, you can explore the model benchmarks to decide which model best fits your needs.

Imagine you want to create a custom copilot that serves as a travel assistant. Specifically, you want your copilot to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms.

Your copilot will need to provide factually accurate information, so groundedness is important. Next to that, you want the copilot's answers to be easy to read and understand. Therefore, you also want to pick a model that is rates high on fluency and coherence.

1. In the Azure AI Studio, navigate to **Model benchmarks** under the **Get started** section, using the menu on the left.
    In the **Quality benchmarks** tab, you can find some charts already visualized for you, comparing different models.
1. Filter the shown models:
    - **Tasks**: Question answering
    - **Collections**: Azure OpenAI
    - **Metrics**: Coherence, Fluency, Groundedness
1. Explore the resulting charts and the comparison table. When exploring, you can try and answer the following questions:
    - Do you notice a difference in performance between GPT-3.5 and GPT-4 models?
    - Is there a difference between versions of the same model?
    - How do the 32k variants differ from the base models?

From the Azure OpenAI collection, you can choose between GPT-3.5 and GPT-4 models. Let's deploy these two models and explore how they compare for your use case.

## Deploy Azure OpenAI models

Now that you have explored your options through model benchmarks, you're ready to deploy language models. You can browse the model catalog, and deploy from there, or you can deploy a model through the **Deployments** page. Let's explore both options.

### Deploy a model from the Model catalog

Let's start by deploying a model from the Model catalog. You may prefer this option when you want to filter through all available models.

1. Navigate to the **Model catalog** page under the **Get started** section, using the menu on the left.
1. Search for and deploy the `gpt-35-turbo` model, curated by Azure AI, with the following settings:
    - **Deployment name**: *A unique name for your model deployment, indicating it's a GPT-3.5 model*
    - **Model version**: *Select the default version*
    - **Deployment type**: Standard
    - **Connected Azure OpenAI resource**: *Select the default connection that was created when you created your hub*
    - **Tokens per Minute Rate Limit (thousands)**: 5K
    - **Content filter**: Default

### Deploy a model through Deployments

If you already know exactly which model you want to deploy, you may prefer to do it through Deployments.

1. Navigate to the **Deployments** page under the **Components** section, using the menu on the left.
1. In the **Model deployments** tab, create a new deployment with the following settings:
    - **Model**: gpt-4
    - **Deployment name**: *A unique name for your model deployment, indicating it's a GPT-4 model*
    - **Model version**: *Select the default version*
    - **Deployment type**: Standard
    - **Connected Azure OpenAI resource**: *Select the default connection that was created when you created your hub*
    - **Tokens per Minute Rate Limit (thousands)**: 5K
    - **Content filter**: Default

> [!NOTE]
> You may have noticed some models showing the Model benchmarks, but not as an option in your model catalog. Model availability differs per location. Your location is specified on the AI hub level, where you can use the **Location helper** to specify the model you want to deploy to get a list of locations you can deploy it in.

## Test your models in the chat playground

Now that we have two models to compare, let's see how the models behave in a conversational interaction.

1. Navigate to the **Chat** page under the **Project playground** section, using the menu on the left.
1. In the **Chat playground**, select your GPT-3.5 deployment.
1. In the chat window, enter the query `What can you do?` and view the response.
    The answers are very generic. Remember we want to create a custom copilot that serves as a travel assistant. You can specify what kind of help you want in the question you ask.
1. In the chat window, enter the query `Imagine you're a travel assistant, what can you help me with?`
    The answers are already more specific. You may not want your end-users to have to provide the necessary context every time they interact with your copilot. To add global instructions, you can edit the system message.
1. Update the system message with the following prompt:

   ```plaintext
   You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms.
   ```

1. Select **Apply changes**, and **Clear chat**.
1. In the chat window, enter the query `What can you do?` and view the new response. Observe how it's different from the answer you received before. The answer is specific to travel now.
1. Continue the conversation by asking: `I'm planning a trip to London, what can I do there?`
    The copilot offers a lot of travel related information. You may want to improve the output still. For example, you may want the answer to be more succinct.
1. Update the system message by adding `Answer with a maximum of two sentences.` to the end of the message. Apply the change, clear the chat, and test the chat again by asking: `I'm planning a trip to London, what can I do there?`
    You may also want your copilot to continue the conversation instead of simply answering the question.
1. Update the system message by adding `End your answer with a follow-up question.` to the end of the message. Apply the change, clear the chat, and test the chat again by asking: `I'm planning a trip to London, what can I do there?`
1. Change your **Deployment** to your GPT-4 model and repeat all steps in this section. Notice how the models may vary in their outputs.
1. Finally, test both models on the query `Who is the prime minister of the UK?`. The performance on this question is related to the groundedness (whether the response is factually accurate) of the models. Does the performance correlate with your conclusions from the Model benchmarks?

Now that you have explored both models, consider what model you would choose now for your use case. At first, the outputs from the models may differ, and you may prefer one model over the other. However, after updating the system message, you may notice that the difference is minimal. From a cost optimization perspective, you may then opt for the GPT-3.5 model over the GPT-4 model, as their performance is very similar.

## Fine-tune a GPT-3.5 model

Before you can fine-tune a model, you need a dataset.

1. Save the training dataset as JSONL file locally: https://raw.githubusercontent.com/MicrosoftLearning/mslearn-ai-studio/main/data/travel-finetune.jsonl
1. Navigate to the **Fine-tuning** page under the **Tools** section, using the menu on the left.
1. Select the button to add a new fine-tune model, select the `gpt-35-turbo` model, and select **Confirm**.
1. **Fine-tune** the model using the following configuration:
    - **Model version**: *Select the default version*
    - **Model suffix**: `ft-travel`
    - **Azure OpenAI connection**: *Select the connection that was created when you created your hub*
    - **Training data**: Upload files
    - **Upload file**: Select the JSONL file you downloaded in a previous step.

> [!TIP]
> You don't have to wait for the data processing to be completed to continue to the next step.

    - **Validation data**: None
    - **Task parameters**: *Keep the default settings*
1. Finetuning will start and may take some time to complete.

> [!NOTE] 
> Fine-tuning and deployment can take some time, so you may need to check back periodically to complete the next step.

## Deploy the fine-tuned model

When fine-tuning has successfully completed, you can deploy the model.

1. Select the fine-tuned model. Select the **Metrics** tab and explore the fine-tune metrics.
1. Deploy the fine-tuned model with the following configurations:
    - **Deployment name**: *A unique name for your model, you can use the default*
    - **Deployment type**: Standard
    - **Tokens per Minute Rate Limit (thousands)**: 5K
    - **Content filter**: Default

## Test the fine-tuned model

Now that you deployed your fine-tuned model, you can test the model like you can test any other deployed model.

1. When the deployment is ready, navigate to the fine-tuned model and select **Open in playground**.
1. In the chat window, enter the query `What can you do?`
    Notice that even though you didn't specify the system message to instruct your model to answer travel-related questions, the model already understands what it should focus on.
1. Try with another query like `Where should I go on holiday for my 30th birthday?`
