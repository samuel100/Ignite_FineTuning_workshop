echo -e "Running the following OLIVE CLI command\n"
set -x
olive quantize \
    --model_name_or_path microsoft/Phi-3.5-mini-instruct \
    --algorithm awq \
    --output_path models/phi/awq \
    --log_level 1
set +x