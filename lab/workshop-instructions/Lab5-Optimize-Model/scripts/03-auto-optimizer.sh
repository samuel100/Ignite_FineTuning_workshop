olive auto-opt \
    --model_name_or_path models/phi/ft/model \
    --adapter_path models/phi/ft/adapter \
    --device cpu \
    --provider CPUExecutionProvider \
    --output_path models/phi/onnx \
    --log_level 1