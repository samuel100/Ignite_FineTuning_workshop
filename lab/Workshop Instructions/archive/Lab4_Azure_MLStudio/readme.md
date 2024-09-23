# Introduction

> [!NOTE]
>This is a **20-minute** exercise, you will successfully fine-tune the Phi-3 model using Azure Machine Learning. Please note that the fine-tuning process can take a considerable amount of time.

## Learning Objectives

By the end of this workshop, you should be able to:

## Lab Scenario
This lab scenario is a continuation of the previous lab, where you fine-tuned the Phi-3 model using the Hugging Face library. In this lab, you will fine-tune the Phi-3 model using Azure Machine Learning Studio and Azure Model Catalog.

## Lab Outline
This lab consists of the following exercises:
1. Fine-tune and Deploy the Phi-3 model in Azure ML Studio


### Fine-tune and Deploy the Phi-3 model in Azure ML Studio
 

Fine-tune the Phi-3 model
In this exercise, you will fine-tune the Phi-3 model in Azure Machine Learning Studio.

In this exercise, you will:

- Create computer cluster for fine-tuning.
- Fine-tune the Phi-3 model in Azure Machine Learning Studio.

### Create computer cluster for fine-tuning

- Visit Azure ML Studio.

- Select Compute from the left side tab.

- Select Compute clusters from the navigation menu.

- Select + New.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-01-select-compute.png) 

Perform the following tasks:

- Select the Region you'd like to use.

- Select the Virtual machine tier to Dedicated.

- Select the Virtual machine type to GPU.

- Select the Virtual machine size filter to Select from all options.

- Select the Virtual machine size to `Standard_NC24ads_A100_v4`.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-02-create-cluster.png)
	
- Select Next.

Perform the following tasks:

- Enter Compute name. It must be a unique value.

- Select the Minimum number of nodes to 0.

- Select the Maximum number of nodes to 1.

- Select the Idle seconds before scale down to 120.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-03-create-cluster.png)

- Select Create.

### Fine-tune the Phi-3 model

Visit Azure ML Studio.[https://ml.azure.com)]

- Select the Azure Machine Learning workspace that you created.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-04-select-workspace.png)

Perform the following tasks:

- Select Model catalog from the left side tab.
- Type phi-3-mini-4k in the search bar and select - Phi-3-mini-4k-instruct from the options that appear.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-05-type-phi-3-mini-4k.png)

- Select Fine-tune from the navigation menu.
![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-06-select-fine-tune.png)

Perform the following tasks:

- Select Select task type to Chat completion.

- Select + Select data to upload Training data.

- Select the Validation data upload type to Provide different validation data.

- Select + Select data to upload Validation data.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-07-fill-finetuning.png)

> [!TIP]
>You can select Advanced settings to customize configurations such as `learning_rate` and `lr_scheduler_type` to optimize the fine-tuning process according to your specific needs.

- Select Finish.

 After running the fine-tuning job, you need to wait for it to complete. You can monitor the status of the fine-tuning job by navigating to the Jobs tab on the left side of your Azure Machine Learning Workspace. In the next series, you will deploy the fine-tuned model and integrate it with Prompt flow.

![](/lab/Workshop%20Instructions/Lab4_Azure_MLStudio/images/06-08-output.png)