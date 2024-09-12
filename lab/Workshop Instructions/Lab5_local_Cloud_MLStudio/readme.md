# Introduction

> [!NOTE]
>This is a **40-minute** workshop that will give you a hands-on introduction to the core concepts and best practices for interacting with OpenAI models.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Use the Azure CLI to authenticate your environment.   
1. Use a Python script to fine-tune a pre-trained model using Azure Machine Learning.
1. Fine-tune a pre-trained model using Azure Machine Learning using a Compute resource.

## Lab Scenario
This lab comprises the following exercises:
Exercise 1: Fine-tune the Phi-3 model using Azure Machine Learning

## Lab Outline
In this exercise, you will fine-tune the Phi-3 model using the provided dataset. First, you will define the fine-tuning process in the fine_tune.py file. Then, you will configure the Azure Machine Learning environment and initiate the fine-tuning process by running the setup_ml.py file. This script ensures that the fine-tuning occurs within the Azure Machine Learning environment.

### Fine-tune the Phi-3 model

By running `setup_ml.py`, you will run the fine-tuning process in the Azure Machine Learning environment.

In this exercise, you will:

- Set up Azure CLI to authenticate environment
- Add code to the fine_tune.py file to fine-tune the model.
- Add code to and run the setup_ml.py file to initiate the fine-tuning process in Azure Machine Learning.
- Run the setup_ml.py file to fine-tune the Phi-3 model using Azure Machine Learning.
 
### Set up Azure CLI
 
You need to set up Azure CLI to authenticate your environment. Azure CLI allows you to manage Azure resources directly from the command line and provides the credentials necessary for Azure Machine Learning to access these resources. To get started install Azure CLI

- Open a terminal window and type the following command to log in to your Azure account.

```bash
az login
```

- Select your Azure account to use.

- Select your Azure subscription to use.

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-1-azlogin.png)

> [!TIP]
>Having trouble signing in to Azure? Try using a device code.Open a terminal window and type the following command to log in to your Azure account. `az login --use-device-code` Visit the website displayed in the terminal window and enter the provided code on that site.

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-2-az-login.png)

- Inside the website, select Next.

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-2-type-code.png)

- Select the account to use in this tutorial

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-3-select-account.png)

- Select continue to complete login.

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-4-select-continue.png)

- After successful login, go back to your terminal and select your Azure subscription to use.

![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-5-select-subscription.png)

### Add code to the `fine_tune.py` file

- Navigate to the finetuning_dir folder and Open the `fine_tune.py` file in Visual Studio Code.

Add the following code into `fine_tune.py.`

```Python
import argparse
import sys
import logging
import os
from datasets import load_dataset
import torch
import mlflow
from transformers import AutoModelForCausalLM, AutoTokenizer, TrainingArguments
from trl import SFTTrainer

# To avoid the INVALID_PARAMETER_VALUE error in MLflow, disable MLflow integration
os.environ["DISABLE_MLFLOW_INTEGRATION"] = "True"

# Logging setup
logging.basicConfig(
    format="%(asctime)s - %(levelname)s - %(name)s - %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    handlers=[logging.StreamHandler(sys.stdout)],
    level=logging.WARNING
)
logger = logging.getLogger(__name__)

def initialize_model_and_tokenizer(model_name, model_kwargs):
    """
    Initialize the model and tokenizer with the given pretrained model name and arguments.
    """
    model = AutoModelForCausalLM.from_pretrained(model_name, **model_kwargs)
    tokenizer = AutoTokenizer.from_pretrained(model_name)
    tokenizer.model_max_length = 2048
    tokenizer.pad_token = tokenizer.unk_token
    tokenizer.pad_token_id = tokenizer.convert_tokens_to_ids(tokenizer.pad_token)
    tokenizer.padding_side = 'right'
    return model, tokenizer

def apply_chat_template(example, tokenizer):
    """
    Apply a chat template to tokenize messages in the example.
    """
    messages = example["messages"]
    if messages[0]["role"] != "system":
        messages.insert(0, {"role": "system", "content": ""})
    example["text"] = tokenizer.apply_chat_template(
        messages, tokenize=False, add_generation_prompt=False
    )
    return example

def load_and_preprocess_data(train_filepath, test_filepath, tokenizer):
    """
    Load and preprocess the dataset.
    """
    train_dataset = load_dataset('json', data_files=train_filepath, split='train')
    test_dataset = load_dataset('json', data_files=test_filepath, split='train')
    column_names = list(train_dataset.features)

    train_dataset = train_dataset.map(
        apply_chat_template,
        fn_kwargs={"tokenizer": tokenizer},
        num_proc=10,
        remove_columns=column_names,
        desc="Applying chat template to train dataset",
    )

    test_dataset = test_dataset.map(
        apply_chat_template,
        fn_kwargs={"tokenizer": tokenizer},
        num_proc=10,
        remove_columns=column_names,
        desc="Applying chat template to test dataset",
    )

    return train_dataset, test_dataset

def train_and_evaluate_model(train_dataset, test_dataset, model, tokenizer, output_dir):
    """
    Train and evaluate the model.
    """
    training_args = TrainingArguments(
        bf16=True,
        do_eval=True,
        output_dir=output_dir,
        eval_strategy="epoch",
        learning_rate=5.0e-06,
        logging_steps=20,
        lr_scheduler_type="cosine",
        num_train_epochs=3,
        overwrite_output_dir=True,
        per_device_eval_batch_size=4,
        per_device_train_batch_size=4,
        remove_unused_columns=True,
        save_steps=500,
        seed=0,
        gradient_checkpointing=True,
        gradient_accumulation_steps=1,
        warmup_ratio=0.2,
    )

    trainer = SFTTrainer(
        model=model,
        args=training_args,
        train_dataset=train_dataset,
        eval_dataset=test_dataset,
        max_seq_length=2048,
        dataset_text_field="text",
        tokenizer=tokenizer,
        packing=True
    )

    train_result = trainer.train()
    trainer.log_metrics("train", train_result.metrics)

    mlflow.transformers.log_model(
        transformers_model={"model": trainer.model, "tokenizer": tokenizer},
        artifact_path=output_dir,
    )

    tokenizer.padding_side = 'left'
    eval_metrics = trainer.evaluate()
    eval_metrics["eval_samples"] = len(test_dataset)
    trainer.log_metrics("eval", eval_metrics)

def main(train_file, eval_file, model_output_dir):
    """
    Main function to fine-tune the model.
    """
    model_kwargs = {
        "use_cache": False,
        "trust_remote_code": True,
        "torch_dtype": torch.bfloat16,
        "device_map": None,
        "attn_implementation": "eager"
    }
    
    pretrained_model_name = "microsoft/Phi-3.5-mini-instruct"
    # pretrained_model_name = "microsoft/Phi-3-mini-4k-instruct"

    with mlflow.start_run():
        model, tokenizer = initialize_model_and_tokenizer(pretrained_model_name, model_kwargs)
        train_dataset, test_dataset = load_and_preprocess_data(train_file, eval_file, tokenizer)
        train_and_evaluate_model(train_dataset, test_dataset, model, tokenizer, model_output_dir)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--train-file", type=str, required=True, help="Path to the training data")
    parser.add_argument("--eval-file", type=str, required=True, help="Path to the evaluation data")
    parser.add_argument("--model_output_dir", type=str, required=True, help="Directory to save the fine-tuned model")
    args = parser.parse_args()
    main(args.train_file, args.eval_file, args.model_output_dir)
``` 

- Save and close the fine_tune.py file.

> [!TIP]
>You can fine-tune Phi-3.5 model. In fine_tune.py file, you can change the pretrained_model_name from "microsoft/Phi-3-mini-4k-instruct" to any model you want to fine-tune. For example, if you change it to "microsoft/Phi-3.5-mini-instruct", you'll be using the Phi-3.5-mini-instruct model for fine-tuning. To find and use the model name you prefer, visit Hugging Face, search for the model you're interested in, and then copy and paste its name into the pretrained_model_name field in your script.

### Add code to the setup_ml.py file

- Open the setup_ml.py file in Visual Studio Code.

- Add the following code into setup_ml.py.

```Python
import logging
from azure.ai.ml import MLClient, command, Input
from azure.ai.ml.entities import Environment, AmlCompute
from azure.identity import AzureCliCredential
from config import (
    AZURE_SUBSCRIPTION_ID,
    AZURE_RESOURCE_GROUP_NAME,
    AZURE_ML_WORKSPACE_NAME,
    TRAIN_DATA_PATH,
    TEST_DATA_PATH
)

# Constants

# Uncomment the following lines to use a CPU instance for training
# COMPUTE_INSTANCE_TYPE = "Standard_E16s_v3" # cpu
# COMPUTE_NAME = "cpu-e16s-v3"
# DOCKER_IMAGE_NAME = "mcr.microsoft.com/azureml/openmpi4.1.0-ubuntu20.04:latest"

# Uncomment the following lines to use a GPU instance for training
COMPUTE_INSTANCE_TYPE = "Standard_NC24ads_A100_v4"
COMPUTE_NAME = "gpu-nc24s-a100-v4"
DOCKER_IMAGE_NAME = "mcr.microsoft.com/azureml/curated/acft-hf-nlp-gpu:59"

CONDA_FILE = "conda.yml"
LOCATION = "eastus2" # Replace with the location of your compute cluster
FINETUNING_DIR = "./finetuning_dir" # Path to the fine-tuning script
TRAINING_ENV_NAME = "phi-3-training-environment" # Name of the training environment
MODEL_OUTPUT_DIR = "./model_output" # Path to the model output directory in azure ml

# Logging setup to track the process
logger = logging.getLogger(__name__)
logging.basicConfig(
    format="%(asctime)s - %(levelname)s - %(name)s - %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    level=logging.WARNING
)

def get_ml_client():
    """
    Initialize the ML Client using Azure CLI credentials.
    """
    credential = AzureCliCredential()
    return MLClient(credential, AZURE_SUBSCRIPTION_ID, AZURE_RESOURCE_GROUP_NAME, AZURE_ML_WORKSPACE_NAME)

def create_or_get_environment(ml_client):
    """
    Create or update the training environment in Azure ML.
    """
    env = Environment(
        image=DOCKER_IMAGE_NAME,  # Docker image for the environment
        conda_file=CONDA_FILE,  # Conda environment file
        name=TRAINING_ENV_NAME,  # Name of the environment
    )
    return ml_client.environments.create_or_update(env)

def create_or_get_compute_cluster(ml_client, compute_name, COMPUTE_INSTANCE_TYPE, location):
    """
    Create or update the compute cluster in Azure ML.
    """
    try:
        compute_cluster = ml_client.compute.get(compute_name)
        logger.info(f"Compute cluster '{compute_name}' already exists. Reusing it for the current run.")
    except Exception:
        logger.info(f"Compute cluster '{compute_name}' does not exist. Creating a new one with size {COMPUTE_INSTANCE_TYPE}.")
        compute_cluster = AmlCompute(
            name=compute_name,
            size=COMPUTE_INSTANCE_TYPE,
            location=location,
            tier="Dedicated",  # Tier of the compute cluster
            min_instances=0,  # Minimum number of instances
            max_instances=1  # Maximum number of instances
        )
        ml_client.compute.begin_create_or_update(compute_cluster).wait()  # Wait for the cluster to be created
    return compute_cluster

def create_fine_tuning_job(env, compute_name):
    """
    Set up the fine-tuning job in Azure ML.
    """
    return command(
        code=FINETUNING_DIR,  # Path to fine_tune.py
        command=(
            "python fine_tune.py "
            "--train-file ${{inputs.train_file}} "
            "--eval-file ${{inputs.eval_file}} "
            "--model_output_dir ${{inputs.model_output}}"
        ),
        environment=env,  # Training environment
        compute=compute_name,  # Compute cluster to use
        inputs={
            "train_file": Input(type="uri_file", path=TRAIN_DATA_PATH),  # Path to the training data file
            "eval_file": Input(type="uri_file", path=TEST_DATA_PATH),  # Path to the evaluation data file
            "model_output": MODEL_OUTPUT_DIR
        }
    )

def main():
    """
    Main function to set up and run the fine-tuning job in Azure ML.
    """
    # Initialize ML Client
    ml_client = get_ml_client()

    # Create Environment
    env = create_or_get_environment(ml_client)
    
    # Create or get existing compute cluster
    create_or_get_compute_cluster(ml_client, COMPUTE_NAME, COMPUTE_INSTANCE_TYPE, LOCATION)

    # Create and Submit Fine-Tuning Job
    job = create_fine_tuning_job(env, COMPUTE_NAME)
    returned_job = ml_client.jobs.create_or_update(job)  # Submit the job
    ml_client.jobs.stream(returned_job.name)  # Stream the job logs
    
    # Capture the job name
    job_name = returned_job.name
    print(f"Job name: {job_name}")

if __name__ == "__main__":
    main()
 

Replace COMPUTE_INSTANCE_TYPE, COMPUTE_NAME, and LOCATION with your specific details.


# Uncomment the following lines to use a GPU instance for training
COMPUTE_INSTANCE_TYPE = "Standard_NC24ads_A100_v4"
COMPUTE_NAME = "gpu-nc24s-a100-v4"
...
LOCATION = "eastus2" # Replace with the location of your compute cluster
```


> [!TIP]
> Guidance for fine-tuning with a minimal dataset using a CPU. If you want to use a CPU for fine-tuning, this approach is ideal for those with benefit subscriptions (such as Visual Studio Enterprise Subscription) or to quickly test the fine-tuning and deployment process. Open the setup_ml file.Replace COMPUTE_INSTANCE_TYPE, COMPUTE_NAME, and DOCKER_IMAGE_NAME with the following. If you do not have access to Standard_E16s_v3, you can use an equivalent CPU instance or request a new quota.Replace LOCATION with your specific details.
 
> [!NOTE]
> Uncomment the following lines to use a CPU instance for training COMPUTE_INSTANCE_TYPE = "Standard_E16s_v3" # cpu COMPUTE_NAME = "cpu-e16s-v3" DOCKER_IMAGE_NAME = "mcr.microsoft.com/azureml/openmpi4.1.0-ubuntu20.04:latest" LOCATION = "eastus2"' Replace with the location of your compute cluster
 
- Type the following command to run the setup_ml.py script and start the fine-tuning process in Azure Machine Learning.

```Bash
python setup_ml.py
```

In this exercise, you successfully fine-tuned the Phi-3 model using Azure Machine Learning. By running the setup_ml.py script, you have set up the Azure Machine Learning environment and initiated the fine-tuning process defined in fine_tune.py file. Please note that the fine-tuning process can take a considerable amount of time. After running the python setup_ml.py command, you need to wait for the process to complete. You can monitor the status of the fine-tuning job by following the link provided in the terminal to the Azure Machine Learning portal.
 
![](/lab/Workshop%20Instructions/Lab5_local_Cloud_MLStudio/images/07-7-see-finetuning-job.png)