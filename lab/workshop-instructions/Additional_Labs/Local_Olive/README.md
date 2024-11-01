# Introduction to using Microsoft OLIVE (ONNX LIVE) 

> [!NOTE]
>This is a **45-minute** workshop that will give you a hands-on introduction to the core concepts and best practices for interacting with models on hugging face.

## Learning Objectives

- Fine Tune a hugging face model using a Jupyter Notebook 
- Testing the fine tuned model
- Models supported in the Notebook 
Hugging Face model name (repo/model)
MODEL="TinyLlama/TinyLlama-1.1B-Chat-v1.0"
MODEL="microsoft/Phi-3.5-mini-instruct"
MODEL="microsoft/meta-llama/Meta-Llama-3.1-8B-Instruct"
MODEL="Qwen/Qwen2-VL-2B-Instruct"

## Lab Scenario

**Minumum Hardware Requirements** Nvidia Tesla T4 Instance in Azure.

This notebook demonstrates how to:

- Fine-tune a model using QLoRA technique
- Optimize the model for inferencing with the [ONNX Runtime](https://onnxruntime.ai/), a cross-platform machine-learning model accelerator, with a flexible interface to integrate hardware-specific libraries.
- Save the base model and adapters seperately for multi-LoRA model serving with ONNX Runtime
- Run the model using the ONNX Runtime


To fine-tune you only need to enter a few arguments into the `olive finetune` command:

- `--method` the method used for fine-tuning. `lora` and `qlora` are supported.
- `--data_name` the Hugging Face dataset name.
- `--text-template` the template to generate text field from. E.g. ‘### Question: {prompt} n### Answer: {response}’. For Phi-3, the chat format is `<|user|>\n{prompt}<|end|>\n<|assistant|>\n{response}<|end|>`
- `--model_name_or_path` The model checkpoint for weights initialization. This can be a Hugging Face model repo, a local path, or an Azure AI Model registry.

More details on available options can be found [here](https://microsoft.github.io/Olive/features/cli.html#finetune).

## Lab Outline
- Using Hugging Face Models
- GPU Finetuning using OLIVE