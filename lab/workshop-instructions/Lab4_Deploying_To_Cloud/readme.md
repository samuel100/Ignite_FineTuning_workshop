# Lab 4. Deploy an AI Model to a cloud endpoint using Azure AI 

## Introduction 

> [!NOTE]
>This is a **45-minute** exercise, you will successfully fine-tune the GPT model using Azure AI one deployment. Please note that the fine-tuning process can take a considerable amount of time.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Deploy the fine-tuned model using Azure AI model deployment.

## Lab Scenario
To integrate the fine-tuned model with an application, you need to deploy the model to make it accessible for real-time inference. This process involves registering the model, creating an online endpoint, and deploying the model.

## Lab Outline
In this exercise, you will:

- Set the model name, endpoint name, and deployment name for deployment.
- Deploy the fine-tuned model in the Azure AI.

## Deploy the fine-tuned model

When fine-tuning has successfully completed, you can deploy the model.

1. Select **Fine-tuning** under **tools**
1. Select your fine-tuned model
1. Select **Deploy**
1. Deploy the fine-tuned model with the following configurations:
    - **Deployment name**: *A unique name for your model deployment*
    - **Deployment type**: Standard
    - **Tokens per Minute Rate Limit (thousands)**: 5K
    - **Content filter**: DefaultV2
    - **Enable dynamic quota**: Disabled
1. Wait for the deployment to be complete before you can test it, this may take a while

## Test the fine-tuned model

Now that you deployed your fine-tuned model and base model, you can test and compare the models responses.

1. When the deployment is ready, navigate to the available models select **Open in playground** and select the model in the **Deployment** drop down.
1. Update the system message with the following instructions:

    ```
    You are an AI travel assistant that helps people plan their trips. Your objective is to offer support for travel-related inquiries, such as visa requirements, weather forecasts, local attractions, and cultural norms.
    You should not provide any hotel, flight, rental car or restaurant recommendations.
    Ask engaging questions to help someone plan their trip and think about what they want to do on their holiday.
    ```
1. Click **Save** or **Apply Changes** to update the system message

1. Test your fine-tuned model to assess whether its behavior is more consistent now. For example, ask the following questions again and explore the model's answers:
   
   ```
   Where in Rome should I stay?
   ```
    
   ```
   Where should i go on Holiday for my 30th Birthday and I love active Sight seeing trips?
   ```
    
