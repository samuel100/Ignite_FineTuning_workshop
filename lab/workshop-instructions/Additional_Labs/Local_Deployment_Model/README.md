# Using ONNX Runtime GenAI to run SLM ONNX model in your local env

> [!NOTE]
>This is a **30-minute** workshop that will give you a hands-on introduction to run SLM onnx model with ORT GenAI .NET

## Learning Objectives


By the end of this workshop, you should be able to:
1. Know basic knowledge of ONNX Runtime GenAI 
2. Compile ONNX Runtime GenAI with CMake
3. Run local model with .NET


### Learn ONNX Runtime GenAI

Run generative AI models with ONNX Runtime.

See the source code here: [https://github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai)

This library provides the generative AI loop for ONNX models, including inference with ONNX Runtime, logits processing, search and sampling, and KV cache management.

Users can call a high level generate() method, or run each iteration of the model in a loop, generating one token at a time, and optionally updating generation parameters inside the loop.

It has support for greedy/beam search and TopP, TopK sampling to generate token sequences and built-in logits processing like repetition penalties. You can also easily add custom scoring.


### Compile ONNX Runtime GenAI with Cmake

Microsoft Phi-3.5 allows us to run smoothly in different edge devices. The local edge devices can have the computing power of CPU, GPU, and NPU. We hope that you can use the local CPU to run our FT computing power. Before running, we need to compile based on the local environment.

1. Set up dev env

   - Visual Studio Code .NET Extension Pack 
   - .NET 8

2. Install CMake in bash


```bash

mkdir libs

cd ./libs

wget https://github.com/Kitware/CMake/releases/download/v3.30.4/cmake-3.30.4.tar.gz

tar -zxvf cmake-3.30.4.tar.gz

cd cmake-3.30.4

sudo apt-get install libssl-dev

./bootstrap

make

sudo make install

```

2. Compile ORT GenAI Library in your local CPU env



```bash

cd ../../

git clone https://github.com/microsoft/onnxruntime-genai

cd onnxruntime-genai

curl -L https://github.com/microsoft/onnxruntime/releases/download/v1.19.2/onnxruntime-linux-x64-1.19.2.tgz -o onnxruntime-linux-x64-1.19.2.tgz

tar xvzf onnxruntime-linux-x64-1.19.2.tgz

mv onnxruntime-linux-x64-1.19.2 ort 

python build.py --config Release

```

3. Using  notebook and select Kernel for .NET to run ONNX model

**Note**： Please download Microsoft Phi-3.5 ONNX model from Hugging face(https://huggingface.co/microsoft/Phi-3.5-mini-instruct-onnx) firstly



```bash

git lfs install

git clone https://huggingface.co/microsoft/Phi-3.5-mini-instruct-onnx

```

write this code in your notebook



```

#r "nuget: Microsoft.ML.OnnxRuntime, 1.19.2"

#r "nuget: Microsoft.ML.OnnxRuntimeGenAI"

using Microsoft.ML.OnnxRuntimeGenAI;

string modelPath = @"Your Microsoft Phi-3.5 onnx model path"; 

var model = new Model(modelPath);

var tokenizer = new Tokenizer(model);

string userPrompt = "What is your name?";

string chatTemplate = $"<|user|>\n{userPrompt}<|end|><|assistant|>\n";

 var tokens = tokenizer.Encode(chatTemplate);

var tokenizerStream = tokenizer.CreateStream();

var generatorParams = new GeneratorParams(model);
generatorParams.SetSearchOption("max_length", 1024);
generatorParams.SetInputSequences(tokens);

var generator = new Generator(model, generatorParams);

while (!generator.IsDone())
{
    generator.ComputeLogits();
    generator.GenerateNextToken();
    Console.Write(tokenizerStream.Decode(generator.GetSequence(0)[^1]));
}


```







