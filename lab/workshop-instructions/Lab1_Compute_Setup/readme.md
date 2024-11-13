# Open yourAI Hub and Project in the Azure AI Portal

You start by creating an Azure AI project within an Azure AI hub:

1. In a web browser, open +++https://ai.azure.com+++ and sign in using your Azure credentials.
1. Select the **Home** 
1. You should a valid **current project** and **Hub**

# Create your Cloud Compute Resources for Cloud based inference

1. Under **Settings**, **Compute**, select **Create Compute**.

  ![Create Azure AI Compute](./images/compute-create.png)

2. Select your **Virtual machine type** as **GPU**. *Filter* the list of **Virtual machine size** on **A100**: 
    
  ![Compte Size](./images/compute-size.png)
    
3. Select a **VM Standard NC24ads_A100v4**  **24 cores**

4. Select **Review+Create**


5. On the Review Screen, we want to ensure the machine doesnt autoshutdown during the lab 

6. Select scheduling and **review** button 

7. Deselect the **Enable idle shutdown**

8. Select **Review+Create**

9. Check the machine size and scheduling 

10. Select **Create**
