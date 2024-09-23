# Optimize model for on-device inference

> [!NOTE]
>This is a **20-minute** workshop that will give you a hands-on introduction to the core concepts of optimizing models for on-device inference using Olive.

## Learning Objectives

By the end of this workshop, you should be able to:

- Quantize an AI Model.
- Fine-tune a quantized model.
- Generate LoRA adapters for efficient on-device inference.

## Lab Scenario
// This is a placeholder for the lab scenario. The scenario should be a brief description of the lab content.

## Lab Outline
This repo provides a tutorial on how to get started with Olive finetuning. You'll need the following:

- An Nvidia GPU device
- A Python installation

## Installation

We recommend creating a new Python environment:

```bash
cd Lab7_Local_Olive
conda create -n -y olive-ai python=3.11
conda activate olive-ai
pip install -r requirements.txt