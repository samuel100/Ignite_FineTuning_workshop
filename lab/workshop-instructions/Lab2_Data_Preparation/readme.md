# Lab2. Data Preperation
## Introduction

> [!NOTE]
>This is a **15-minute** workshop that will give you a hands-on introduction to the core concepts and best practices for data preparation.

## Learning Objectives

By the end of this workshop, you should be able to: 
1. Prepare a dataset for fine-tuning with Azure Machine Learning.
1. Download the dataset using the download_dataset.py file.
1. Verify that the datasets were saved successfully to your local finetune-phi/data directory.
1. Understand the importance of dataset preparation in the machine learning process.

## Lab Scenario
This lab will guide you through the process of fine-tuning a pre-trained model using Azure Machine Learning. In this lab, you will prepare the dataset for fine-tuning with Azure Machine Learning. You will download the 'ultrachat_200k datasets' to your local environment and use this dataset to fine-tune the Phi-3 model in Azure Machine Learning.

## Lab Outline
This lab comprises the following exercises:
Exercise 1: Prepare Dataset for Fine-tuning with Azure Machine Learning 

### Prepare Dataset for Fine-tuning

In this exercise, you will run the download_dataset.py file to download the 'ultrachat_200k datasets' to your local environment. You will then use this datasets to fine-tune the Phi-3 model in Azure Machine Learning.

In this exercise, you will:

Add code to the 'download_dataset.py' file to download the datasets.
Run the 'download_dataset.py' file to download datasets to your local environment.
 
### Download your dataset using download_dataset.py

- Create a new Python file  `download_dataset.py` file in Visual Studio Code.

- Add the following code into `download_dataset.py`.

```Python
import json
import os
from datasets import load_dataset
from config import (
    TRAIN_DATA_PATH,
    TEST_DATA_PATH)

def load_and_split_dataset(dataset_name, config_name, split_ratio):
    """
    Load and split a dataset.
    """
    # Load the dataset with the specified name, configuration, and split ratio
    dataset = load_dataset(dataset_name, config_name, split=split_ratio)
    print(f"Original dataset size: {len(dataset)}")
    
    # Split the dataset into train and test sets (80% train, 20% test)
    split_dataset = dataset.train_test_split(test_size=0.2)
    print(f"Train dataset size: {len(split_dataset['train'])}")
    print(f"Test dataset size: {len(split_dataset['test'])}")
    
    return split_dataset

def save_dataset_to_jsonl(dataset, filepath):
    """
    Save a dataset to a JSONL file.
    """
    # Create the directory if it does not exist
    os.makedirs(os.path.dirname(filepath), exist_ok=True)
    
    # Open the file in write mode
    with open(filepath, 'w', encoding='utf-8') as f:
        # Iterate over each record in the dataset
        for record in dataset:
            # Dump the record as a JSON object and write it to the file
            json.dump(record, f)
            # Write a newline character to separate records
            f.write('\n')
    
    print(f"Dataset saved to {filepath}")

def main():
    """
    Main function to load, split, and save the dataset.
    """
    # Load and split the ULTRACHAT_200k dataset with a specific configuration and split ratio
    dataset = load_and_split_dataset("HuggingFaceH4/ultrachat_200k", 'default', 'train_sft[:1%]')
    
    # Extract the train and test datasets from the split
    train_dataset = dataset['train']
    test_dataset = dataset['test']

    # Save the train dataset to a JSONL file
    save_dataset_to_jsonl(train_dataset, TRAIN_DATA_PATH)
    
    # Save the test dataset to a separate JSONL file
    save_dataset_to_jsonl(test_dataset, TEST_DATA_PATH)

if __name__ == "__main__":
    main()
``` 

> [!TIP]
>Guidance for fine-tuning with a minimal dataset using a CPU If you want to use a CPU for fine-tuning, this approach is ideal for those with benefit subscriptions (such as Visual Studio Enterprise Subscription) or to quickly test the fine-tuning and deployment process. Replace `dataset = load_and_split_dataset("HuggingFaceH4/ultrachat_200k", 'default', 'train_sft[:1%]')` with `dataset = load_and_split_dataset("HuggingFaceH4/ultrachat_200k", 'default', 'train_sft[:10]')`

 
- Type the following command inside your terminal to run the script and download the dataset to your local environment.

```Python
python download_dataset.py
```

- Verify that the datasets were saved successfully to your local finetune-phi/data directory.


> [!NOTE]
>Note on dataset size and fine-tuning time. In this tutorial, you use only 1% of the dataset (train_sft[:1%]). This significantly reduces the amount of data, speeding up both the upload and fine-tuning processes. You can adjust the percentage to find the right balance between training time and model performance. Using a smaller subset of the dataset reduces the time required for fine-tuning, making the process more manageable for a tutorial.
