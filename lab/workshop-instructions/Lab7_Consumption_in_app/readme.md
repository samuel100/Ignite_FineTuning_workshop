# Lab 7. Consumption of your Model within an Application 

## Introduction

> [!NOTE]
>This is a **10-minute** workshop that will give you a hands-on introduction to the core concepts of using a code first approach to consuming a deployed model endpoint.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Integrate the custom Phi-3 model with Prompt flow.
1. Test your custom Phi-3 model on Prompt flow.

## Lab Scenario
This lab scenario comprimise of the following:    
- Integrate the custom Phi-3 model with Prompt flow
- Test your custom Phi-3 model on Prompt flow
- Set api key and endpoint uri of the fine-tuned Phi-3 model.
- Add code to the `flow.dag.yml` file.
- Add code to the `integrate_with_promptflow.py` file.
- Test your custom Phi-3 model on Prompt flow

## Lab Outline
This lab includes the following exercises:
1. Integrate the custom Phi-3 model with Prompt flow
1. Test your custom Phi-3 model on Prompt flow


### Exercise 1. Integrate the custom Phi-3 model with Prompt flow

After successfully deploying your fine-tuned model, you can now integrate it with Prompt Flow to use your model in real-time applications, enabling a variety of interactive tasks with your custom Phi-3 model.

In this exercise, you will:

- Set api key and endpoint uri of the fine-tuned Phi-3 model.
- Add code to the `flow.dag.yml` file.
- Add code to the `integrate_with_promptflow.py` file.
- Test your custom Phi-3 model on Prompt flow.
 

### Set api key and endpoint uri of the fine-tuned Phi-3 model

- Navigate to the Azure Machine learning workspace that you created.
-Select Endpoints from the left side tab.
![Screenshot select endpoint](./images/11-select-endpoints.png)
-Select endpoint that you created.
![screeshot of endpoints available](./images/11-select-endpoint-created.png)

- Select Consume from the navigation menu.

- Copy and paste your REST endpoint into the `config.py` file, replacing `AZURE_ML_ENDPOINT = "your_fine_tuned_model_endpoint_uri"` with your REST endpoint.

- Copy and paste your Primary key into the `config.py` file, replacing `AZURE_ML_API_KEY = "your_fine_tuned_model_api_key"` with your Primary key.

![Screenshot of API Keys](./images/11-copy-apikey-endpoint.png)	

### Add code to the flow.dag.yml file

- Open the `flow.dag.yml` file in Visual Studio Code.

Add the following code into flow.`dag.yml`.

```Python
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
 
Add code to the integrate_with_promptflow.py file
```
 

- Open the `integrate_with_promptflow.py` file in Visual Studio Code.

Add the following code into `integrate_with_promptflow.py`.

```Python
import logging
import requests
from promptflow.core import tool
import asyncio
import platform
from config import (
    AZURE_ML_ENDPOINT,
    AZURE_ML_API_KEY
)

# Logging setup
logging.basicConfig(
    format="%(asctime)s - %(levelname)s - %(name)s - %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    level=logging.DEBUG
)
logger = logging.getLogger(__name__)

def query_azml_endpoint(input_data: list, endpoint_url: str, api_key: str) -> str:
    """
    Send a request to the Azure ML endpoint with the given input data.
    """
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
        result = response.json()[0]
        logger.info("Successfully received response from Azure ML Endpoint.")
        return result
    except requests.exceptions.RequestException as e:
        logger.error(f"Error querying Azure ML Endpoint: {e}")
        raise

def setup_asyncio_policy():
    """
    Setup asyncio event loop policy for Windows.
    """
    if platform.system() == 'Windows':
        asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())
        logger.info("Set Windows asyncio event loop policy.")

@tool
def my_python_tool(input_data: str) -> str:
    """
    Tool function to process input data and query the Azure ML endpoint.
    """
    setup_asyncio_policy()
    return query_azml_endpoint(input_data, AZURE_ML_ENDPOINT, AZURE_ML_API_KEY)
```
 

- Type the following command to run the integrate_with_promptflow script and start Prompt flow.

```Bash
pf flow serve --source ./ --port 8080 --host localhost
```

Here's an example of the results: Now you can chat with your custom Phi-3 model. It is recommended to ask questions based on the data used for fine-tuning.

![Screenshot of example deployment](./images/11-1-promptflow-example.png)



### Exercise 2. Running fine-tuned Phi-3 model in local event

Now that you have successfully run the application in the cloud, let's see how to run it on a local device.

In this exercise, you will:

- using ONNX Runtime GenAI to call fine-tuned Phi-3.5 model
- using .NET Aspire to create local Copilot Solution

if you want to learn 

1. Learn to set ONNX Runtime GenAI env [click here](../Additional_Labs/Local_Deployment_Model/readme.md)

2. Learn to .NET Aspire env [click here](../Additional_Labs/dotNETAspire/readme.md)

3. Set env

   - Set NodeJS env

   ```Python

   curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash

   nvm --version

   nvm intsall 20

   nvm use 20 

   ```

4. Running Scripts

### Navigate to the Project Directory:
```
cd ./script/Phi3DotNETAspire/Phi3.Aspire.AppHost
```

### Build the Project:

```
dotnet build
```

### Copy Necessary Libraries:

Ensure you are in the .../src/04.CloudNativeRAG/Phi3DotNETAspire/Phi3.Aspire.AppHost folder (You can copy the files manually or use the following command)

Ensure you are in the 'src' folder 
```

cp ./script/libs/onnxruntime-genai/build/Linux/RelWithDebInfo/libonnxruntime-genai.so ./script/Phi3DotNETAspire/Phi3.Aspire.ModelService/bin/Debug/net8.0/runtimes/linux-x64/native/

cp ./script/libs/onnxruntime-genai/build/Linux/RelWithDebInfo/libonnxruntime.so ./script/Phi3DotNETAspire/Phi3.Aspire.ModelService/bin/Debug/net8.0/runtimes/linux-x64/native/

```

### Set Environment Variable:
```
export ASPIRE_ALLOW_UNSECURED_TRANSPORT=true
```

### Run the Application:
```
dotnet run --launch-profile http
```

### Accessing the .NET Aspire Portal
Click the Follow Link in the terminal to open the .NET Aspire Portal using the localhost link: 

Eample of the follow link in the info section of the output in your terminal 

```
Login to the dashboard at http://localhost:15147/login?t=65d752d2a8345d9f3t5656ef78e4777
```


### Enter the token

You will be prompted to enter a login token this can be found in your terminal output.

Example will be as follows

```
Login to the dashboard at http://localhost:15147/login?t=65d752d2a8345d9f3t5656ef78e4777
```

In this case the login code is

```
65d752d2a8345d9f3f10680ef78e4777
```

### View the Portal:
Setting Up Vue Portal in Codespaces
Open up the brower windows and you will see the .NET Aspire Portal with a list of services and ports, select the vue services. You will see Endpoint as per the example below http://localhost:42811, copy port number 42811

![PortSettings](./images/0302.png)

### Configure Your Ports: 
You know need to configure your GitHub Codespaces ports. 
To set up port forwarding for http://localhost:42811 in GitHub Codespaces, follow these steps:

![Configure Ports](./images/0303.png)

**Access the PORTS tab:**

- If you’re using Visual Studio Code, click on the PORTS tab in the bottom panel.
- If you’re using the browser, you can find the PORTS tab in terminal window.

**Add the port:**

You can manually forward a port that wasn't forwarded automatically.

- Open the terminal in your codespace.
- Click the PORTS tab.

Under the list of ports, click Add port.
- Click on Add port.
- Enter 42811 as the port number and press Enter.

**Access the forwarded port:**

Once the port is forwarded, you can access it via a URL provided by GitHub Codespaces. This URL will be displayed in the PORTS tab and can be clicked to open in your browser.

**Change port protocol:**

If you need to use HTTPS instead of HTTP, right-click the port in the PORTS tab, hover over Change Port Protocol, and select HTTPS.
For more detailed information, you can refer to the GitHub Docs on forwarding ports in [Codespaces](https://docs.github.com/en/codespaces/developing-in-a-codespace/forwarding-ports-in-your-codespace)

### Chat with Phi-3.5

**Start Chatting:** 

- You need to open the newly created port 
![Created Port]((./imgs/0306.png)
- In the terminal select the newly created port forwarding address and select open browser
![OpenBrowser](./images/0305.png)

You can now start to chat
![Chat with Phi-3](./images/0304.png)








