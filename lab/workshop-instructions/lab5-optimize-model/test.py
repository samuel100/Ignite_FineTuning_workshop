import onnxruntime_genai as og
import numpy as np
from olive.common.utils import load_weights
import os

model_folder = "models/ft-onnx/model"

# Load the base model and tokenizer
model = og.Model(model_folder)
tokenizer = og.Tokenizer(model)
tokenizer_stream = tokenizer.create_stream()

# Load the LoRA adapter weights
weights_file = os.path.join(model_folder, "adapter_weights.onnx_adapter")

adapters = {
    "travel": {
        "weights": weights_file,
        "template": "<|user|>\n{input}</s>\n<|assistant|>"
    }
}

adapters_weights = {
    key: load_weights(value["weights"]) for key, value in adapters.items()
}

# Set the max length to something sensible by default,
# since otherwise it will be set to the entire context length
search_options = {}
search_options['max_length'] = 200
search_options['past_present_share_buffer'] = False

chat_template = "<|user|>\n{input}</s>\n<|assistant|>"

text = input("Input: ")

# Keep asking for input phrases
while text != "exit":
  if not text:
    print("Error, input cannot be empty")
    exit

  # generate prompt (prompt template + input)
  prompt = f'{chat_template.format(input=text)}'

  # encode the prompt using the tokenizer
  input_tokens = tokenizer.encode(prompt)

  # the adapter weights are added to the model at inference time. This means you
  # can select different adapters for different tasks i.e. multi-LoRA.

  params = og.GeneratorParams(model)
  for k, v in adapter_weights.items():
    params.set_model_input(k, v)
  params.set_search_options(**search_options)
  params.input_ids = input_tokens
  generator = og.Generator(model, params)

  print("Output: ", end='', flush=True)
  # stream the output
  try:
    while not generator.is_done():
      generator.compute_logits()
      generator.generate_next_token()

      new_token = generator.get_next_tokens()[0]
      print(tokenizer_stream.decode(new_token), end='', flush=True)
  except KeyboardInterrupt:
      print("  --control+c pressed, aborting generation--")

  print()
  text = input("Input: ")