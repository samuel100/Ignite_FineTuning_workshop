# Lab 7. Consumption of your Model within an Application 

## Introduction

> [!NOTE]
>This is a **10-minute** workshop that will give you a hands-on introduction to the core concepts of using a code first approach to consuming a deployed model endpoint.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Integrate the custom fined Phi local model with the application.
1. Intergrate the custom fine tuned GPT Model with the application.
2. Compare the results of the models

## Lab Scenario
In this lab you utilise a console application to validate the following scenarios 
1. Chat with Phi-3.5 ONNX unoptizimed fine tuned model, Phi-3.5 ONNX OLIVE Optimized fine tuned Model and GPT-.3.5 fine tuned Model - To this will allow you to compare the resulting messages.
2. Evaluation of the Phi-3.5 ONNX Olive Fine tuned Model 

## Setup 

### Create a new folder on the desktop and clone the code using a command prompt.

Open a new Powershell terminal window

``` 
PS C:\Users\LabUser>
```

### Download the solution 

```
cd desktop
mkdir Application
cd Application
git clone https://github.com/Azure/Ignite_FineTuning_workshop.git
```

### In the application folder navigate to the solution 

```
cd Ignite_FineTuning_workshop/lab/workshop-instructions/Lab7_Consumption_in_app/scr
ipts/ChatWithSLM.Console
```

## Open the solution in VScode 

```
code
```

## Running fine-tuned GPT model in the cloud 

Go to ChatWithSLM.Console/Utils/GenAI.cs , add your onnx models path, save

![location](./images/location.png)


## Running console application


```

dotnet run

```

You can choose two different scenarios

1. Experience of different models based on travel data

2. Experience of models before and after optimization

As shown in the figure


![result](./images/result.png)
