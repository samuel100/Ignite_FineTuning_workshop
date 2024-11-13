# Open yourAI Hub and Project in the Azure AI Portal

You start by creating an Azure AI project within an Azure AI hub:

1. In a web browser, open +++https://ai.azure.com+++ and sign in using your Azure credentials.
1. Select the **Home** 
1. You should a valid **current project** and **Hub**

# Create your Cloud Compute Resources for Cloud based inference

1. Under **Settings**, **Compute**, select **Create Compute**.

  ![Create Azure AI Compute](./images/compute-create.png)

1. Select your **Virtual machine type** as **GPU**. *Filter* the list of **Virtual machine size** on **A100**: 
    
  ![Compte Size](./images/compute-size.png)
    
    Select a **VM Standard NC24ads_A100v4**  **24 cores**

1. Select **Review+Create**

On the Review Screen 

We want to ensure the machine doesnt autoshutdown during the lab 

Select scheduling and **review** button 

Deselect the **Enable idle shutdown**

1. Select **Review+Create**

Check the machine size and scheduling 

1. Select **Create**
