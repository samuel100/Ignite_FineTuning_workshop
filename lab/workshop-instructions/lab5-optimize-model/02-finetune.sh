olive finetune \
    --method lora \
    --model_name_or_path models/phi/awq \
    --trust_remote_code \
    --data_files "data/data_sample_travel.jsonl" \
    --data_name "json" \
    --text_template "<|user|>\n{prompt}<|end|>\n<|assistant|>\n{response}<|end|>" \
    --max_steps 15 \
    --output_path ./models/ft \
    --target_modules o_proj,kv_proj \
    --log_level 1
