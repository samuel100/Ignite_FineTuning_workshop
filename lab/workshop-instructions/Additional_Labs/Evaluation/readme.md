
## Integrate the custom model with Prompt flow in Azure AI Studio(Code first approach)
 
### Create Azure AI Studio Hub

You need to create a Hub before creating the Project. A Hub acts like a Resource Group, allowing you to organize and manage multiple Projects within Azure AI Studio.

- Sign in [Azure AI Studio](https://ai.azure.com).

- Select All hubs from the left side tab.

- Select + New hub from the navigation menu.

![Screenshot Create an AI Studio Hub for Evaluation](./images/7_create-hub.png)

Perform the following tasks:

- Enter Hub name. It must be a unique value.
- Select your Azure Subscription.
- Select the Resource group to use (create a new one if needed).
- Select the Location you'd like to use.
- Select the Connect Azure AI Services to use (create a new one if needed).
- Select Connect Azure AI Search to Skip connecting.

![Screenshot of hub settings](./images/8_fill-hub.png)

- Select Next.

### Create Azure AI Studio Project
 
In the Hub that you created, select All projects from the left side tab.

- Select + New project from the navigation menu.
![Screenshot of creating and AI Hub](./images/9_select-new-project.png)

- Enter Project name. It must be a unique value.
![Screenshot of seeting up an AI project](./images/10_create-project.png)

- Select Create a project.

### Add a custom connection for the fine-tuned Phi-3 / Phi-3.5 model

To integrate your custom Phi-3 / Phi-3.5 model with Prompt flow, you need to save the model's endpoint and key in a custom connection. This setup ensures access to your custom Phi-3 / Phi-3.5 model in Prompt flow.

- Set api key and endpoint uri of the fine-tuned Phi-3 / Phi-3.5 model

- Visit [Azure ML Studio](https://ml.azure.com).

- Navigate to the Azure Machine learning workspace that you created.

- Select Endpoints from the left side tab.

![Screenshot selecting endpoins](./images/11_select-endpoints.png) 

- Select endpoint that you created.

![screenshot of endpoint selection](./images/12_select-endpoint-created.png)

- Select Consume from the navigation menu.

- Copy your REST endpoint and Primary key.
![](./images/13_copy-endpoint-key.png)

- Add the Custom Connection

- Visit [Azure AI Studio](https://ai.azure.com).

- Navigate to the Azure AI Studio project that you created.

- In the Project that you created, select Settings from the left side tab.

- Select + New connection.

![screenshot of creating new connection](./images/14_select-new-connection.png)

- Select Custom keys from the navigation menu.
![Screenshot of creating keys](./images/15_select-custom-keys.png)

Perform the following tasks:

- Select + Add key value pairs.
For the key name, enter endpoint and paste the endpoint you copied from Azure ML Studio into the value field.
- Select + Add key value pairs again.
For the key name, enter key and paste the key you copied from Azure ML Studio into the value field.
- After adding the keys, select is secret to prevent the key from being exposed.

![Screenshot of adding the connection](./images/16_add-connection.png)

- Select Add connection.

### Create Prompt flow

You have added a custom connection in Azure AI Studio. Now, let's create a Prompt flow using the following steps. Then, you will connect this Prompt flow to the custom connection to use the fine-tuned model within the Prompt flow.

- Navigate to the Azure AI Studio project that you created.

- Select Prompt flow from the left side tab.

- Select + Create from the navigation menu.

![Screenshot Create new prompt flow](./images/17_select-promptflow.png)

- Select Chat flow from the navigation menu.

![Screenshot of Selection of the flow](./images/18_select-flow-type.png)

- Enter Folder name to use.
![Screenshot of creating a new prompt flow](./images/19_enter-name.png)

- Select Create.

### Set up Prompt flow to chat with your custom model
 You need to integrate the fine-tuned model into a Prompt flow. However, the existing Prompt flow provided is not designed for this purpose. Therefore, you must redesign the Prompt flow to enable the integration of the custom model.

In the Prompt flow, perform the following tasks to rebuild the existing flow:

- Select Raw file mode.

- Delete all existing code in the `flow.dag.yml` file.

Add the folling code to `flow.dag.yml`.

```
inputs:
  input_data:
    type: string
    default: "Who founded Microsoft?"

outputs:
  answer:
    type: string
    reference: ${integrate_with_promptflow.output}

nodes:
- name: integrate_with_promptflow
  type: python
  source:
    type: code
    path: integrate_with_promptflow.py
  inputs:
    input_data: ${inputs.input_data}
```
- Select Save.

![Screenshot of modifying the flow](./images/20_select-raw-file-mod.png)

Add the following code to integrate_with_promptflow.py to use the custom model in Prompt flow.

``` 
import logging
import requests
from promptflow import tool
from promptflow.connections import CustomConnection

# Logging setup
logging.basicConfig(
    format="%(asctime)s - %(levelname)s - %(name)s - %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    level=logging.DEBUG
)
logger = logging.getLogger(__name__)

def query_phi3_model(input_data: str, connection: CustomConnection) -> str:
    """
    Send a request to the model endpoint with the given input data using Custom Connection.
    """

    # "connection" is the name of the Custom Connection, "endpoint", "key" are the keys in the Custom Connection
    endpoint_url = connection.endpoint
    api_key = connection.key

    headers = {
        "Content-Type": "application/json",
        "Authorization": f"Bearer {api_key}"
    }
data = {
    "input_data": [input_data],
    "params": {
        "temperature": 0.7,
        "max_new_tokens": 128,
        "do_sample": True,
        "return_full_text": True
        }
    }
    try:
        response = requests.post(endpoint_url, json=data, headers=headers)
        response.raise_for_status()
        
        # Log the full JSON response
        logger.debug(f"Full JSON response: {response.json()}")

        result = response.json()["output"]
        logger.info("Successfully received response from Azure ML Endpoint.")
        return result
    except requests.exceptions.RequestException as e:
        logger.error(f"Error querying Azure ML Endpoint: {e}")
        raise

@tool
def my_python_tool(input_data: str, connection: CustomConnection) -> str:
    """
    Tool function to process input data and query the Phi-3 / Phi-3.5 model.
    """
    return query_phi3_model(input_data, connection)

```

> [!NOTE]
>For more detailed information on using Prompt flow in Azure AI Studio, you can refer to Prompt flow in Azure AI Studio.

- Select Chat input, Chat output to enable chat with your model.

![Screenshot of the output](./images/21_select-input-output.png)

Now you are ready to chat with your custom Phi-3 / Phi-3.5 model. In the next exercise, you will learn how to start Prompt flow and use it to chat with your fine-tuned Phi-3 / Phi-3.5 model.

> [!NOTE]
> The rebuilt flow should look like the image below:
![Screenshot of chart/graph output](./images/22_graph-example.png)

### Start Prompt flow

Select Start compute sessions to start Prompt flow.

![Screenshot of starting the prompt flow session](./images/23_start-compute-session.png)

- Select Validate and parse input to renew parameters.

![Screenshot of validating the flow](./images/24_validate-input.png)

- Select the Value of the connection to the custom connection you created. For example, connection.

![Screenshot of selecting the flow](./images/25_select-connection.png)

### Chat with your custom Phi-3 / Phi-3.5 model
 
- Select Chat.
![Screenshot of chat interaction with the model](./images/26_select-chat.png)

Here's an example of the results: Now you can chat with your custom model. It is recommended to ask questions based on the data used for fine-tuning.
![screenshot of prompt flow](./images/27_chat-with-promptflow.png)

### Deploy Azure OpenAI to evaluate the model

To evaluate the model in Azure AI Studio, you need to deploy an Azure OpenAI model. This model will be used to evaluate the performance of the model.

### Deploy Azure OpenAI

- Sign in to [Azure AI Studio](https://ai.azure.com).

Navigate to the Azure AI Studio project that you created. 

![Screenshot of project deployment](./images/28_select-project-created.png)

In the Project that you created, select Deployments from the left side tab.

- Select + Deploy model from the navigation menu.

- Select Deploy base model.
![Screenshot selecting to deploy a model](./images/29_deploy-openai-model.png)

- Select Azure OpenAI model you'd like to use. For example, gpt-4o.
![Screenshot selecting model type to deploy](./images/31_select-openai-model.png)	

- Select Confirm.

### Evaluate the fine-tuned model using Azure AI Studio's Prompt flow evaluation


- Start a new evaluation
- Visit [Azure AI Studio](https://ai.azure.com).

- Navigate to the Azure AI Studio project that you created.
![Screenshot showing AI Studio](./images/32_select-project-created.png)

In the Project that you created, select Evaluation from the left side tab.

- Select + New evaluation from the navigation menu. 

![Screenshot of selecting model evaluation](./images/33_select-evaluation.png)

- Select Prompt flow evaluation.
![screenshot of prompt flow evaluation](./images/34_promptflow-evaluation.png)


Perform the following tasks:

- Enter the evaluation name. It must be a unique value.
Select Question and answer without context as the task type. Because, the UlTRACHAT_200k dataset used in this tutorial does not contain context.
- Select the prompt flow you'd like to evaluate.

![screenshot of evaluation settings](./images/35_evaluation-setting.png)	
 

- Select Next.

Perform the following tasks:

- Select Add your dataset to upload the dataset. For example, you can upload the test dataset file, such as test_data.json1, which is included when you download the ULTRACHAT_200k dataset.
- Select the appropriate Dataset column that matches your dataset. For example, if you are using the ULTRACHAT_200k dataset, select ${data.prompt} as the dataset column.

![Screenshot of Evaluation result](./images/36_evaluation-setting.png)
- Select Next.

Perform the following tasks to configure the performance and quality metrics:

- Select the performance and quality metrics you'd like to use.
- Select the Azure OpenAI model that you created for evaluation. For example, select gpt-4o.
![Screenshot of evaluation metrics](./images/37_evaluation-setting.png)

Perform the following tasks to configure the risk and safety metrics:

- Select the risk and safety metrics you'd like to use.
- Select the threshold to calculate the defect rate you'd like to use. For example, select Medium.
- For question, select Data source to {$data.prompt}.
- For answer, select Data source to {$run.outputs.answer}.
- For ground_truth, select Data source to {$data.message}.
![screenshot of evaluation output](./images/38_evaluation-setting.png)

- Select Next.

- Select Submit to start the evaluation.

The evaluation will take some time to complete. You can monitor the progress in the Evaluation tab.

### Review the Evaluation Results
 

> [!NOTE]
>The results presented below are intended to illustrate the evaluation process. In this tutorial, we have used a model fine-tuned on a relatively small dataset, which may lead to sub-optimal results. Actual results may vary significantly depending on the size, quality, and diversity of the dataset used, as well as the specific configuration of the model.

Once the evaluation is complete, you can review the results for both performance and safety metrics.

Performance and quality metrics:

- evaluate the model’s effectiveness in generating coherent, fluent, and relevant responses.
![Screenshot of Coherent, fluent and relevant responses](./images/39_evaluation-result-gpu.png)

Risk and safety metrics:

- Ensure that the model’s outputs are safe and align with Responsible AI Principles, avoiding any harmful or offensive content.
![screenshot of results and if the contain harmful responses](./images/40_evaluation-result-gpu.png)

You can scroll down to view Detailed metrics result.
![Screenshot of detailed metrics](./images/41_detailed-metrics-result.png)