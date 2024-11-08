# Lab 7. Consumption of your Model within an Application 

## Introduction

> [!NOTE]
>This is a **10-minute** workshop that will give you a hands-on introduction to the core concepts of using a code first approach to consuming a deployed model endpoint.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Integrate the custom fined Phi local model with the application.
1. Intergrate the custom fine tuned GPT Model with the applciation.
1. Compare the results of the models
1. Demonstrate offline model inference
1. Demonstrate local and cloud inference

## Lab Scenario
Use ONNX Runtime for On-device, Use Azure AI for Cloud.


## Setup 
```
mkdir Application
cd application
git clone https://github.com/Azure/Ignite_FineTuning_workshop.git
```
## Open the solution in VScode

```
cd Ignite_FineTuning_workshop



## Running fine-tuned GPT model in the cloud 

Needs to be updated for calling GPT3.5 Fine tuned model 

## Running fine-tuned Phi local model application

Now that you have successfully run the application in the cloud, let's see how to run it on a local device.

In this exercise, you will:

- using ONNX Runtime GenAI to call fine-tuned model
- using .NET Aspire to create local Copilot Solution

### Objectives 

1. Learn to set ONNX Runtime GenAI env [click here](../Additional_Labs/Local_Deployment_Model/readme.md)

2. Learn more about .NET Aspire env [click here](../Additional_Labs/dotNETAspire/readme.md)

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

### Chat with Fine Tuned Phi Model

**Start Chatting:** 

- You need to open the newly created port 
![Created Port]((./imgs/0306.png)
- In the terminal select the newly created port forwarding address and select open browser
![OpenBrowser](./images/0305.png)

You can now start to chat
![Chat with Phi-3](./images/0304.png)


### Change your fine-tuning model path

Please go to scripts\Phi3DotNETAspire\Phi3.Aspire.ModelService change model Path to check your fine tuning model with ONNX model








