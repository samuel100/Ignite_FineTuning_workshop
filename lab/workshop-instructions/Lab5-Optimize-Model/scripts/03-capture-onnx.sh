echo -e "Running the following OLIVE CLI command\n"
set -x

olive capture-onnx-graph \
    --model_name_or_path models/phi/ft/model \
    --adapter_path models/phi/ft/adapter \
    --use_ort_genai \
    --output_path models/phi/onnx \
    --log_level 1

ls -lah models/phi/onnx/model

set +x