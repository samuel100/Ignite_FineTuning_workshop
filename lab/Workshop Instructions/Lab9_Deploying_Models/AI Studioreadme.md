# Introduction

> [!NOTE]
>This is a **20-minute** exercise, you will successfully fine-tune the Phi-3 model using Azure Machine Learning using the UI/GUI. Please note that the fine-tuning process can take a considerable amount of time.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Deploy the fine-tuned model using the portal.


## Lab Scenario
To integrate the fine-tuned Phi-3 model with an application, you need to deploy the model to make it accessible for real-time inference. This process involves registering the model, creating an online endpoint, and deploying the model.

## Lab Outline
In this exercise, you will:

- Register the fine-tuned model in the Azure Machine Learning workspace.
- Create an online endpoint.
- Deploy the registered fine-tuned Phi-3 model.


### Register the fine-tuned model
 

Visit [Azure ML Studio](https://ml.azure.com).

- Select the Azure Machnine Learning workspace that you created.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/06-04-select-workspace.png)

- Select Models from the left side tab.

- Select + Register.

- Select From a job output.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-01-register-model.png) 

- Select the job that you created.
![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-02-select-job.png)

- Select Next.

- Select Model type to MLflow.

- Ensure that Job output is selected; it should be automatically selected.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-03-select-output.png

- Select Next.

- Select Register.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-04-register.png)

- You can view your registered model by navigating to the Models menu from the left side tab.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-05-registered-model.png)

### Deploy the fine-tuned model
 
- Navigate to the Azure Machine Learning workspace that you created.

- Select Endpoints from the left side tab.

- Select Real-time endpoints from the navigation menu.
![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-06-create-endpoint.png)

- Select Create.

- select the registered model that you created.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-07-select-registered-model.png)

- Select Select.

Perform the following tasks:

- Select Virtual machine to `Standard_NC6s_v3`.

- Select the Instance count you'd like to use. For example, 1.

- Select the Endpoint to New to create an endpoint.

- Enter Endpoint name. It must be a unique value.

- Enter Deployment name. It must be a unique value.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-08-deployment-setting.png) 

- Select Deploy.

> [!WARNING]
>To avoid additional charges to your account, make sure to delete the created endpoint in the Azure Machine Learning workspace.

 
### Check deployment status in Azure Machine Learning Workspace
 

Navigate to Azure Machine Learning workspace that you created.

Select Endpoints from the left side tab.

Select the endpoint that you created.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-09-check-deployment.png)

- On this page, you can manage the endpoints during the deployment process.

> [!NOTE]
> Once the deployment is complete, ensure that Live traffic is set to 100%. If it is not, select Update traffic to adjust the traffic settings. Note that you cannot test the model if the traffic is set to 0%.

![](/lab/Workshop%20Instructions/Lab8_Deploying_Models/images/07-10-set-traffic.png)