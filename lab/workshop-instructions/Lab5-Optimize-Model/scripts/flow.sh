echo -e "\n>>>>>> running awq quantization >>>>>>>>\n"

olive quantize \
    --model_name_or_path azureml://registries/azureml-meta/models/Llama-3.2-1B-Instruct/versions/1 \
    --algorithm awq \
    --output_path models/llama/awq \
    --log_level 1

echo -e "\n>>>>>> running finetuning >>>>>>>>\n"

olive finetune \
    --method lora \
    --model_name_or_path models/llama/awq \
    --trust_remote_code \
    --data_files "data/data_sample_travel.jsonl" \
    --data_name "json" \
    --text_template "<|user|>\n{prompt}<|end|>\n<|assistant|>\n{response}<|end|>" \
    --max_steps 15 \
    --output_path ./models/llama/ft \
    --log_level 1

echo -e "\n>>>>>> running optimizer >>>>>>>>\n"

olive auto-opt \
    --model_name_or_path models/llama/ft/model \
    --adapter_path models/llama/ft/adapter \
    --device cpu \
    --provider CPUExecutionProvider \
    --use_ort_genai \
    --output_path models/llama/onnx-ao \
    --log_level 1
