# Lab 5. Optimize AI models for on-device inference

## Introduction 

> [!IMPORTANT]
> This lab requires an **Nvidia A10 or A100 GPU** with associated drivers and CUDA toolkit (version 12+) installed.

> [!NOTE]
> This is a **30-minute** lab that will give you a hands-on introduction to the core concepts of optimizing models for on-device inference using OLIVE.

## Learning Objectives

By the end of this lab, you will be able to use OLIVE to:

- Quantize an AI Model using the AWQ quantization method.
- Fine-tune an AI model for a specific task.
- Generate LoRA adapters (fine-tuned model) for efficient on-device inference on the ONNX Runtime.

### What is OLIVE

OLIVE (ONNX LIVE) is a model optimization toolkit with accompanying CLI that enables you to ship models for the ONNX runtime +++https://onnxruntime.ai+++ with quality and performance.

![Olive Flow](./images/olive-flow.png)

The input to OLIVE is typically a PyTorch or Hugging Face model and the output is an optimized ONNX model that is executed on a device (deployment target) running the ONNX runtime. OLIVE will optimize the model for the deployment target's AI accelerator (NPU, GPU, CPU) provided by a hardware vendor such as Qualcomm, AMD, Nvidia or Intel.

OLIVE executes a *workflow*, which is an ordered sequence of individual model optimization tasks called *passes* - example passes include: model compression, graph capture, quantization, graph optimization. Each pass has a set of parameters that can be tuned to achieve the best metrics, say accuracy and latency, that are evaluated by the respective evaluator. OLIVE employs a search strategy that uses a search algorithm to auto-tune each pass one by one or set of passes together.

#### Benefits of OLIVE

- **Reduce frustration and time** of trial-and-error manual experimentation with different techniquies for graph optimization, compression and quantization. Define your quality and performance constraints and let OLIVE automatically find the best model for you.
- **40+ built-in model optimization components** covering cutting edge techniques in quantization, compression, graph optimization and finetuning.
- **Easy-to-use CLI** for common model optimization tasks. For example, olive quantize, olive auto-opt, olive finetune.
- Model packaging and deployment built-in.
- Supports generating models for **Multi LoRA serving**.
- Construct workflows using YAML/JSON to orchestrate model optimization and deployment tasks.
- **Hugging Face** and **Azure AI** Integration.
- Built-in **caching** mechanism to **save costs**.

## Lab Instructions
> [!NOTE]
> Please ensure you have provision your Azure AI Hub and Project as per Lab 1.

### Step 0: Connect to your Azure AI Compute

You'll connect to the Azure AI compute using the remote feature in VS Code. Open your VS Code desktop application:

1. Open the **command palette** using  **Shift+Ctrl+P**
1. In the command palette search for **AzureML - remote: Connect to compute instance in New Window**.
1. Follow the on-screen instructions to connect to the Compute. This will involve selecting your Azure Subscription, Resource Group, Project and Compute name you set up in Lab 1.

### Step 1: Clone this repo

In VS Code, you can open a new terminal with **Ctrl+J** and clone this repo:

```bash
cd ~/cloudfiles/code
git clone https://github.com/Azure/Ignite_FineTuning_workshop.git
```

### Step 2: Open Folder in VS Code

To open VS Code in the relevant folder execute the following command in the terminal, which will open a new window:

```bash
code Ignite_FineTuning_workshop/lab/workshop-instructions/Lab5-Optimize-Model
```

Alternatively, you can open the folder by selecting **File** > **Open Folder**. 

### Step 3: Dependencies

Open a terminal window in VS Code in your Azure AI Compute Instance (tip: **Ctrl+J**) and execute the following commands to install the dependencies:

```bash
conda create -n olive-ai python=3.11 -y
conda activate olive-ai
pip install -r requirements.txt
sudo apt-get -y install cudnn9-cuda-12
```

In this lab you'll download and upload models to the Azure AI Model catalog. So that you can access the model catalog, you'll need to login to Azure using:

```bash
az login
```

> [!NOTE]
> At login time you'll be asked to select your subscription. Ensure you set the subscription to the one provided for this lab.

### Step 4: Execute OLIVE commands 

Open a terminal window in VS Code in your Azure AI Compute Instance (tip: **Ctrl+J**) and ensure the `olive-ai` conda environment is activated:

```bash
conda activate olive-ai
```

Next, execute the following Olive commands in the command line.

1. **Inspect the data:** In this example, you're going to fine-tune Phi-3.5-Mini model so that it is specialized in answering travel related questions. The code below displays the first few records of the dataset, which are in JSON lines format:
   
    ```bash
    head data/data_sample_travel.jsonl
    ```
1. **Quantize the model:** Before training the model, you first quantize with the following command that uses a technique called Active Aware Quantization (AWQ) +++https://arxiv.org/abs/2306.00978+++, which provides more accurate results than the Round to Nearest (RTN) technique:
    
    ```bash
    olive quantize \
        --model_name_or_path azureml://registries/azureml/models/Phi-3.5-mini-instruct/versions/4 \
        --algorithm awq \
        --output_path models/phi/awq \
        --log_level 1
    ```
    
    It takes **~5mins** to complete the AWQ quantization.

1. **Train the model:** Next, the `olive finetune` command finetunes the quantized model. We find that quantizing the model *before* fine-tuning greatly improves the accuracy.
    
    ```bash
    olive finetune \
        --method lora \
        --model_name_or_path models/phi/awq \
        --trust_remote_code \
        --data_files "data/data_sample_travel.jsonl" \
        --data_name "json" \
        --text_template "<|user|>\n{prompt}<|end|>\n<|assistant|>\n{response}<|end|>" \
        --max_steps 100 \
        --output_path ./models/phi/ft \
        --log_level 1
    ```
    
    It takes **~10mins** to complete the Fine-tuning (depending on the number of epochs).Olive supports the following models out-of-the-box: Phi, Llama, Mistral, Gemma, Qwen, Falcon and [many others +++https://huggingface.co/docs/optimum/en/exporters/onnx/overview+++. For more information on available options, read the Olive Finetune documentation +++https://microsoft.github.io/Olive/features/cli.html#finetune+++.

1. **Capture ONNX Graph:** With the model trained, you need to capture the ONNX graph, which will add the adapter nodes into the graph.

    ```bash
    olive capture-onnx-graph \
        --model_name_or_path models/phi/ft/model \
        --adapter_path models/phi/ft/adapter \
        --use_ort_genai \
        --output_path models/phi/onnx \
        --log_level 1
    ```

1. **Generate adapters:** The following command will change the adapter nodes of the ONNX graph into inputs and saves the weights in a separate file:
    
    ```bash
    olive generate-adapter \
        --model_name_or_path models/phi/onnx \
        --output_path models/phi/ft-ready \
        --log_level 1
    ```
    
    It takes **~2mins** to complete the adapter extraction and ONNX optimization.

### Step 5: Model inference quick test

To test inferencing the model, create a Python file in your folder called **app.py** and copy-and-paste the following code:

```python
import onnxruntime_genai as og
import numpy as np

model = og.Model("models/phi/ft-ready/model")
adapters = og.Adapters(model)
adapters.load("models/phi/ft-ready/model/adapter_weights.onnx_adapter", "travel")

tokenizer = og.Tokenizer(model)
tokenizer_stream = tokenizer.create_stream()

params = og.GeneratorParams(model)
params.set_search_options(max_length=100, past_present_share_buffer=False)
params.input_ids = tokenizer.encode("Tell me what to do in London")

generator = og.Generator(model, params)

generator.set_active_adapter(adapters, "travel")

print(f"[Travel]: Tell me what to do in London")

while not generator.is_done():
    generator.compute_logits()
    generator.generate_next_token()

    new_token = generator.get_next_tokens()[0]
    print(tokenizer_stream.decode(new_token), end='', flush=True)

print("\n")
```

Execute the code using:

```bash
python app.py
```

### Step 6: Upload model to Azure AI

Uploading the model to an Azure AI model repository makes the model sharable with other members of your development team and also handles version control of the model. To upload the model run the following command:

> [!NOTE]
> Update the `{}` placeholders with the name of your resource group and Azure AI Project Name.

```bash
az ml model create \
    --name ft-for-travel \
    --version 1 \
    --path ./models/phi/ft-ready \
    --resource-group {RESOURCE_GROUP_NAME} \
    --workspace-name {PROJECT_NAME}
```
