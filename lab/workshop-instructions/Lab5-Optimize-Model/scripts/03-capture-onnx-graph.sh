olive capture-onnx-graph \
    --model_name_or_path models/phi/ft/model \
    --adapter_path models/phi/ft/adapter \
    --use_ort_genai \
    --output_path models/phi/onnx \
    --log_level 1
