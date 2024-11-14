# Lab 7. Consumption of your Model within an Application 

## Introduction

> [!NOTE]
>This is a **10-minute** workshop that will give you a hands-on introduction to the core concepts of using a code first approach to consuming a deployed model endpoint.

## Learning Objectives

By the end of this workshop, you should be able to:
1. Integrate the custom fined Phi local model with the application.
1. Intergrate the custom fine tuned GPT Model with the applciation.
2. Compare the results of the models

## Lab Scenario
This is console application


## Setup 

Create a new folder on the desktop and clone the code using a command prompt.

Open a new Powershell terminal window
 
PS C:\Users\LabUser>

### Download the solution 

```
cd desktop
mkdir Application
cd application
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