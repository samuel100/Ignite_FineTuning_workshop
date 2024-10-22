echo -e "Running the following OLIVE CLI command\n"
set -x

olive generate-adapter \
    --model_name_or_path models/phi/onnx \
    --output_path models/phi/ft-ready \
    --log_level 1

ls -lah models/phi/ft-ready/model

set +x