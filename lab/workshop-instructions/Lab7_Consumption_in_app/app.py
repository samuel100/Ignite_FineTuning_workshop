import os
import time
import onnxruntime_genai as og
from openai import AzureOpenAI
from dotenv import load_dotenv

model = og.Model("../Lab5-Optimize-Model/models/phi/onnx-ao/model")
adapters = og.Adapters(model)
adapters.load("../Lab5-Optimize-Model/models/phi/onnx-ao/model/adapter_weights.onnx_adapter", "travel")

tokenizer = og.Tokenizer(model)
tokenizer_stream = tokenizer.create_stream()

# open AI deployment information
load_dotenv()

endpoint = os.getenv("ENDPOINT_URL")  
deployment = os.getenv("DEPLOYMENT_NAME")  
subscription_key = os.getenv("AZURE_OPENAI_API_KEY")  

# Initialize Azure OpenAI client with key-based authentication
client = AzureOpenAI(  
    azure_endpoint=endpoint,  
    api_key=subscription_key,  
    api_version="2024-05-01-preview"
)        

while True:
    params = og.GeneratorParams(model)
    params.set_search_options(max_length=100, past_present_share_buffer=False)
    
    text = input("Input: ")
    
    params.input_ids = tokenizer.encode(f"<|user|>\n{text}<|end|>\n<|assistant|>\n")

    # base case
    generator = og.Generator(model, params)
    print("\n**********************************************")
    print("Base Phi-3.5 model:")    
    print("**********************************************\n")
    tks = 0
    st = time.time()
    while not generator.is_done():
        generator.compute_logits()
        generator.generate_next_token()

        new_token = generator.get_next_tokens()[0]
        print(tokenizer_stream.decode(new_token), end='', flush=True)
        tks +=1
    et = time.time()
    del generator
    print(f"\n\nTokens/sec:{tks/(et-st)}")
    # with adapter
    generator = og.Generator(model, params)
    generator.set_active_adapter(adapters, "travel")
    print("\n\n**********************************************")
    print("With adapter: ")    
    print("**********************************************\n")
    tks = 0
    st = time.time()
    while not generator.is_done():
        generator.compute_logits()
        generator.generate_next_token()

        new_token = generator.get_next_tokens()[0]
        print(tokenizer_stream.decode(new_token), end='', flush=True)
        tks +=1
    et = time.time()
    del generator
    print(f"\n\nTokens/sec:{tks/(et-st)}")
    print("\n")
    
    # Azure Open AI
    print("\n\n**********************************************")
    print("Open AI - Finetuned: ")    
    print("**********************************************\n")
    stream = client.chat.completions.create(
        messages=[
            {
                "role": "user",
                "content": text,
            }
        ],
        model=deployment,
        stream=True,
        stream_options={"include_usage": True}, 
    )
    
    st = time.time()
    for chunk in stream:
        if len(chunk.choices) > 0:
            print(chunk.choices[0].delta.content, end="", flush=True)
    et = time.time()
    print("\n")
    print(f"\n\nTokens/sec:{chunk.usage.completion_tokens/(et-st)}")
    print("\n")

