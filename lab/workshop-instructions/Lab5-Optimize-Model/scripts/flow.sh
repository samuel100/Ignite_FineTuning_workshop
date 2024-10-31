echo -e "\n>>>>>> running awq quantization >>>>>>>>\n"

olive quantize \
    --model_name_or_path azureml://registries/azureml/models/Phi-3.5-mini-instruct/versions/4 \
    --algorithm awq \
    --output_path models/phi/awq \
    --log_level 1

echo -e "\n>>>>>> running finetuning >>>>>>>>\n"

olive finetune \
    --method lora \
    --model_name_or_path models/phi/awq \
    --trust_remote_code \
    --data_files "data/data_sample_travel.jsonl" \
    --data_name "json" \
    --text_template "<|user|>\n{prompt}<|end|>\n<|assistant|>\n{response}<|end|>" \
    --max_steps 15 \
    --output_path ./models/phi/ft \
    --log_level 1

echo -e "\n>>>>>> running graph capture >>>>>>>>\n"

olive capture-onnx-graph \
    --model_name_or_path models/phi/ft/model \
    --adapter_path models/phi/ft/adapter \
    --use_ort_genai \
    --output_path models/phi/onnx \
    --log_level 1

echo -e "\n>>>>>> generating adapters >>>>>>>>\n"

olive generate-adapter \
    --model_name_or_path models/phi/onnx \
    --output_path models/phi/ft-ready \
    --log_level 1

