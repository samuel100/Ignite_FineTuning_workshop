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
